using BBComponents.Abstract;
using BBComponents.Enums;
using BBComponents.Helpers;
using BBComponents.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;


namespace BBComponents.Components
{
    public partial class BbComboBox<TValue> : ComponentBase, IDisposable
    {
        private const int InputTimerInterval = 500;
        private const int DefaultDropdownWidth = 250;

        private TValue _value;
        private string _inputValue;
        private string _inputValueTmp;
        private string _searchString;
        private bool _isOpen;
        private bool _isAddOpen;
        private bool _stopListenOnInputValueChange;

        private bool _isCustomMenuOpen;
        private double _clientX;
        private double _clientY;

        private List<IMenuItem> _menuItems = new List<IMenuItem>();


        private ElementReference _inputElementReference;
        private HtmlElementInfo _inputElementInfo;

        private Timer _timer;

        private List<SelectItem<TValue>> _source = new List<SelectItem<TValue>>();
        private bool disposedValue;

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

        [Parameter]
        public bool UseCustomMenu { get; set; }

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
        /// Event for add new.
        /// </summary>
        [Parameter]
        public EventCallback<ComboBoxOpenArgs<TValue>> OpenClicked { get; set; }

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


        /// <summary>
        /// Placeholder for input.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }


        [Parameter]
        public bool AllowAdd { get; set; }

        [Parameter]
        public bool AllowAddWhenDisabled { get; set; }


        [Parameter]
        public DropdownPositions DropdownPosition { get; set; } = DropdownPositions.Absolute;

        [Parameter]
        public int DropdownWidth { get; set; } = DefaultDropdownWidth;

        public string SizeClass => HtmlClassBuilder.BuildSizeClass("input-group", Size);

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
                    var width = DropdownWidth > 0 ? DropdownWidth : DefaultDropdownWidth;

                    return $"{width}px";
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

                    foreach (var item in _source)
                    {
                        if (string.IsNullOrWhiteSpace(item.Text))
                        {
                            continue;
                        }

                        var flagAdd = true;
                        foreach (var part in parts)
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

        protected override void OnInitialized()
        {
            _menuItems = new List<IMenuItem>();

            if (AllowAdd)
            {
                _menuItems.Add(new MenuItem()
                {
                    Title = $"Add",
                    Name = $"add",
                    Kind = MenuItemKinds.Item,
                    IconClass = "fa fa-plus text-primary",
                    HotKeyTooltip = "Alt+A"
                });
            }

            _menuItems.Add(new MenuItem()
            {
                Title = $"Open",
                Name = $"open",
                Kind = MenuItemKinds.Item,
                IconClass = "fa fa-search text-primary",
                HotKeyTooltip = "Alt+O"
            });

            _menuItems.Add(new MenuItem()
            {
                Title = $"Clear",
                Name = $"clear",
                Kind = MenuItemKinds.Item,
                IconClass = "fa fa-times text-danger",
                HotKeyTooltip = "Alt+X"

            });

            _menuItems.Add(new MenuItem() { Kind = BBComponents.Enums.MenuItemKinds.Delimiter });

            _menuItems.Add(new MenuItem()
            {
                Title = $"Close",
                Name = $"close",
                Kind = MenuItemKinds.Item,
                IconClass = "fa fa-times text-secondary"
            });

        }

        protected override void OnParametersSet()
        {
            _value = Value;

            if (!string.IsNullOrEmpty(Text))
            {
                _inputValue = Text;
            }
            else
            {
                if (ItemsSource != null)
                {
                    var firstItem = ItemsSource.FirstOrDefault();

                    if (firstItem != null)
                    {
                        var propValue = firstItem.GetType().GetProperty(ValueName);
                        var propText = firstItem.GetType().GetProperty(TextName);

                        foreach (var item in ItemsSource)
                        {

                            var value = (TValue)propValue?.GetValue(item);

                            if (EqualityComparer<TValue>.Default.Equals(value, _value))
                            {
                                var text = propText?.GetValue(item)?.ToString();
                                _inputValue = text;
                                break;
                            }
                        }
                    }
                }
            }

        }

        private async Task OnOpenClick(MouseEventArgs args)
        {

            _inputElementInfo = await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _inputElementReference);

            _searchString = "";
            _isOpen = !_isOpen;
            _isAddOpen = false;

            if (_isOpen)
            {
                FillSource();
            }
        }

        private async Task OnClearClick()
        {
            await Clear();
        }



        private void OnInput(ChangeEventArgs e)
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
                FillSource();
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
                    _value = item.Value;
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

        private async Task OnInputKeyDown(KeyboardEventArgs e)
        {
            if (e.AltKey == true && e.Code == "KeyO")
            {
                await Open();

            }
            else if (e.AltKey == true && e.Code == "KeyX")
            {

                if (IsDisabled)
                {
                    return;
                }

                await Clear();

            }
            else if (e.AltKey == true && e.Code == "KeyA")
            {
                await OnAddNewClick();
            }
        }

        private async Task OnItemClick(MouseEventArgs e, SelectItem<TValue> item)
        {
            _inputValue = item.Text;
            _value = item.Value;

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
            if (!AllowAdd)
            {
                return;
            }

            if (IsDisabled && !AllowAddWhenDisabled)
            {
                return;
            }

            _isAddOpen = false;
            _isOpen = false;

            var args = new ComboBoxAddNewArgs()
            {
                Id = Id,
                Text = _inputValueTmp
            };
            await AddNewClicked.InvokeAsync(args);
        }

        private void OnContextMenu(MouseEventArgs e)
        {
            if (!UseCustomMenu)
            {
                return;
            }

            _clientX = e.ClientX;
            _clientY = e.ClientY;

            _isCustomMenuOpen = true;

        }

        private async Task OnContextMenuClosed(IMenuItem item)
        {
            _isCustomMenuOpen = false;

            if (item.Name == "clear")
            {
                if (IsDisabled)
                {
                    return;
                }

                await Clear();
            }
            else if (item.Name == "open")
            {
                await Open();
            }
            else if (item.Name == "add")
            {
                await OnAddNewClick();
            }

        }

        private async Task Clear()
        {
            _inputValue = "";
            _value = default(TValue);
            _searchString = "";

            var defaultValue = default(TValue);
            await TextChanged.InvokeAsync("");
            await ValueChanged.InvokeAsync(defaultValue);
            await Changed.InvokeAsync(defaultValue);
        }

        private async Task Open()
        {
            if (EqualityComparer<TValue>.Default.Equals(_value, default(TValue)))
            {
                return;
            }

            var openArgs = new ComboBoxOpenArgs<TValue>()
            {
                Id = Id,
                Text = _inputValue,
                Value = _value
            };

            await OpenClicked.InvokeAsync(openArgs);
        }

        private void FillSource()
        {

            if (ItemsSource == null)
            {
                return;
            }

            //if (_source.Count() == ItemsSource.Count())
            //{
            //    return;
            //}

            _source = new List<SelectItem<TValue>>();

            var firstItem = ItemsSource.FirstOrDefault();

            if (firstItem != null)
            {
                var propValue = firstItem.GetType().GetProperty(ValueName);
                var propText = firstItem.GetType().GetProperty(TextName);

                foreach (var item in ItemsSource)
                {

                    var value = (TValue)propValue?.GetValue(item);
                    var text = propText?.GetValue(item)?.ToString();

                    var isDeleted = false;

                    if (!string.IsNullOrEmpty(IsDeletedName))
                    {
                        var propIsDeleted = item.GetType().GetProperty(IsDeletedName);
                        var isDeletedValue = propIsDeleted?.GetValue(item);
                        isDeleted = isDeletedValue == null ? false : (bool)isDeletedValue;
                    }

                    _source.Add(new SelectItem<TValue>(text, value, isDeleted));
                }
            }


        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_source != null)
                    {
                        _source = null;
                    }

                    if (_timer != null)
                    {
                        _timer = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
