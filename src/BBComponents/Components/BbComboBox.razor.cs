using BBComponents.Enums;
using BBComponents.Helpers;
using BBComponents.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;


namespace BBComponents.Components
{
    public partial class BbComboBox<TValue>: ComponentBase
    {
        private const int InputTimerInterval = 500;

        private string _inputValue;
        private string _inputValueTmp;
        private string _searchString;
        private bool _isOpen;
        private bool _isAddOpen;
        private bool _stopListenOnInputValueChange;

        private ElementReference _inputElementReference;
        private HtmlElementInfo _inputElementInfo;

        private Timer _timer;

        private List<SelectItem<TValue>> _source = new List<SelectItem<TValue>>();

        [Inject]
        public IJSRuntime JsRuntime { get; set; }


        [Parameter]
        public string Id { get; set; }

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
        /// Event for add new.
        /// </summary>
        [Parameter] 
        public EventCallback<ComboBoxAddNewArgs> AddNewClicked { get; set; }

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

        /// <summary>
        /// Indicates that component is disabled.
        /// </summary>
        [Parameter]
        public bool IsDisabled { get; set; }


        [Parameter]
        public bool AllowAdd { get; set; }

        [Parameter]
        public DropdownPositions DropdownPosition { get; set; } = DropdownPositions.Absolute;

        [Parameter]
        public int DropdownWidth { get; set; } = 250;

        public string SizeClass => HtmlClassBuilder.BuildSizeClass("input-group", Size);

        public bool IsValueSelected => EqualityComparer<TValue>.Default.Equals(Value, default(TValue));

        public string DropdownPositionValue
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Absolute)
                {
                    return "absolute";
                }
                else if (DropdownPosition == DropdownPositions.Fixed)
                {
                    return "fixed";
                }
                else
                {
                    return "absolute";
                }
            }
        }

        public string DropdownWidthValue
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
                {
                    return $"{DropdownWidth}px";
                }

                return "100%";
            }
        }

        public string DrowdownTop
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
                {

                    return $"{_inputElementInfo.TopInt}px";
                }

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

        public string DropdownMarginTop
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
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

                return "0";
            }
        }

        public string DropdownLeft
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
                {
                    return $"{_inputElementInfo.LeftInt}px";
                }

                return "1px";
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

                //if (AllowAdd)
                //{
                //    _isAddOpen = !sourceFiltered.Any();
                //    if (_isAddOpen)
                //    {
                //        _isOpen = false;
                //    }
                //}


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

        private async Task OnOpenClick(MouseEventArgs args)
        {

           _inputElementInfo =  await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _inputElementReference);

            _searchString = "";
            _isOpen = !_isOpen;
            _isAddOpen = false;
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

        private async Task OnInputValueChange(ChangeEventArgs e)
        {
            //Debug.WriteLine($"ValueChange: {e.Value}");

            //if (_stopListenOnInputValueChange)
            //{
            //    _stopListenOnInputValueChange = false;
            //    return;
            //}

            //_inputValue = e.Value?.ToString();
            //_searchString = _inputValue;



        }

        private async Task OnInput(ChangeEventArgs e)
        {
            Debug.WriteLine($"Input: {e.Value}");

            if (IsDisabled)
            {
                return;
            }

            _inputValueTmp = e.Value?.ToString();

            if (_timer != null)
            {
                _timer.Stop();
            }

            // Try to search items only when input stopped or interupted
            _timer = new Timer();
            _timer.Elapsed += async (s, args) => await OnTimerTick();
            _timer.Interval = InputTimerInterval;
            _timer.Enabled = true;
            _timer.Start();


        }

        private async Task OnTimerTick()
        {
            Debug.WriteLine($"Timer tick: {_inputValueTmp}");
            _timer.Stop();

            _searchString = _inputValueTmp;

            if (!_isOpen)
            {
                _inputElementInfo = await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _inputElementReference);
                _isOpen = true;
            }


            if (SourceFiltered.Count == 0)
            {
                if (AllowAdd)
                {
                    _isAddOpen = true;
                    _isOpen = false;
                }
                else
                {
                    _isAddOpen = false;
                }
            }
            else
            {
                _isAddOpen = false;
            }

           await InvokeAsync(() => { this.StateHasChanged(); });
        }

        private async Task OnInputKeyPress(KeyboardEventArgs e)
        {
            if (IsDisabled)
            {
                return;
            }

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
                    _isAddOpen = false;
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
            _isAddOpen = false;
            _searchString = "";
        }

        private void OnCancelAddNewClick()
        {
            _isAddOpen = false;
            _isOpen = false;
            _inputValue = "";
        }

        private async Task OnAddNewClick()
        {
            _isAddOpen = false;
            _isOpen = false;

            var args = new ComboBoxAddNewArgs()
            {
                Id = Id,
                Text = _inputValue
            };
            await AddNewClicked.InvokeAsync(args);
        }
    }
}
