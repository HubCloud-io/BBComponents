using BBComponents.Enums;
using BBComponents.Helpers;
using BBComponents.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbComboBox<TValue>: ComponentBase
    {

        private string _inputValue;
        private string _searchString;
        private bool _isOpen;
        private bool _stopListenOnInputValueChange;
        private List<SelectItem<TValue>> _source = new List<SelectItem<TValue>>();

        [Parameter]
        public BootstrapElementSizes Size { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Tooltip for component.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Duplicate event call back for value changed. 
        /// It is necessary to have possibility catch changed even whet we use @bind-Value.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> Changed { get; set; }

        /// <summary>
        /// Event for text changed.
        /// </summary>
        [Parameter]
        public EventCallback<string> TextChanged { get; set; }

        /// <summary>
        /// Colllection for select options.
        /// </summary>
        [Parameter]
        public IEnumerable<object> ItemsSource { get; set; }

        /// <summary>
        /// Name of property with option Value.
        /// </summary>
        [Parameter]
        public string ValueName { get; set; }

        /// <summary>
        /// Name of property with option IsDeleted.
        /// </summary>
        [Parameter]
        public string IsDeletedName { get; set; }

        /// <summary>
        /// Name of property with option Text.
        /// </summary>
        [Parameter]
        public string TextName { get; set; }

        public string SizeClass => HtmlClassBuilder.BuildSizeClass("input-group", Size);

        public bool IsValueSelected => EqualityComparer<TValue>.Default.Equals(Value, default(TValue));

        public string TopDrowdown
        {
            get
            {
                int topValue;
                switch (Size)
                {
                    case BootstrapElementSizes.Sm:
                        topValue = 32;
                        break;
                    case BootstrapElementSizes.Lg:
                        topValue = 49;
                        break;
                    default:
                        topValue = 39;
                        break;
                }

                return $"{topValue}px";
            }
        }

        public List<SelectItem<TValue>> SourceFiltered
        {
            get
            {
                var sourceFiltered = new List<SelectItem<TValue>>();

                if (string.IsNullOrEmpty(_searchString))
                {
                    sourceFiltered = _source;
                }
                else
                {
                    var searchString = _searchString.ToLower().Trim();
                    var parts = searchString.Split(' ');
                    
                    foreach(var item in _source)
                    {
                        if (string.IsNullOrWhiteSpace(item.Text))
                        {
                            continue;
                        }

                        var flagAdd = true;
                        foreach(var part in parts)
                        {
                            if (!item.Text.ToLower().Contains(part))
                            {
                                flagAdd = false;
                                break;
                            }
                        }

                        if (flagAdd)
                        {
                            sourceFiltered.Add(item);
                        }
                    }

                }

                return sourceFiltered;
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            _source = new List<SelectItem<TValue>>();

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    var propValue = item.GetType().GetProperty(ValueName);
                    var propText = item.GetType().GetProperty(TextName);

                    var value = (TValue) propValue?.GetValue(item);
                    var text = propText?.GetValue(item)?.ToString();

                    var isDeleted = false;

                    if (!string.IsNullOrEmpty(IsDeletedName))
                    {
                        var propIsDeleted = item.GetType().GetProperty(IsDeletedName);
                        var isDeletedValue = propIsDeleted?.GetValue(item);
                        isDeleted = isDeletedValue == null ? false : (bool)isDeletedValue;
                    }

                    _source.Add(new SelectItem<TValue>(text,value, isDeleted));
                }
            }

            // Set initial value
            var selectedItem = _source.FirstOrDefault(x => EqualityComparer<TValue>.Default.Equals(x.Value, Value));

            if (selectedItem != null)
            {
                
                if (_inputValue != selectedItem.Text)
                {
                    _inputValue = selectedItem.Text;
                    await TextChanged.InvokeAsync(_inputValue);
                }

            }
            else if (string.IsNullOrEmpty(Text))
            {
                _inputValue = Text;
            }

        }

        private void OnOpenClick()
        {
            _searchString = "";
            _isOpen = !_isOpen;
        }

        private async Task OnClearClick()
        {
            _inputValue = "";
            _searchString = "";

            var defaultValue = default(TValue);
            await TextChanged.InvokeAsync("");
            await ValueChanged.InvokeAsync(defaultValue);
            await Changed.InvokeAsync(defaultValue);


        }

        private void OnInputValueChange(ChangeEventArgs e)
        {
            if (_stopListenOnInputValueChange)
            {
                _stopListenOnInputValueChange = false;
                return;
            }

            _inputValue = e.Value?.ToString();
            _searchString = _inputValue;
        }

        private void OnInput(ChangeEventArgs e)
        {
            _inputValue = e.Value?.ToString();
            _searchString = _inputValue;

            if (!_isOpen)
            {
                _isOpen = true;
            }
        }

        private async Task OnInputKeyPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                if (SourceFiltered.Count == 1)
                {
                    _stopListenOnInputValueChange = true;

                    var item = SourceFiltered[0];

                    _inputValue = item.Text;
                    StateHasChanged();
                    await TextChanged.InvokeAsync(item.Text);
                    await ValueChanged.InvokeAsync(item.Value);
                    await Changed.InvokeAsync(item.Value);

                    _isOpen = false;
                    _searchString = "";

                }
            }
        }

        private async Task OnItemClick(MouseEventArgs e, SelectItem<TValue> item)
        {
            _inputValue = item.Text;
            await TextChanged.InvokeAsync(item.Text);
            await ValueChanged.InvokeAsync(item.Value);
            await Changed.InvokeAsync(item.Value);

            _isOpen = false;
            _searchString = "";
        }
    }
}
