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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbDatePicker : ComponentBase
    {
        private double _windowWidth;
        private double _windowHeight;
        
        private string _stringValue;
        private bool _isOpen;
        private string _monthName;
        private int _year;
        private int _month;
        private string[] _dayNames;

        private ElementReference _inputElementReference;
        private HtmlElementInfo _inputElementInfo;
        private string _inputGroupKey;


        private CultureInfo _culture;
        private List<List<CalendarDay>> _weeks;

        private bool _isCustomMenuOpen;
        private double _clientX;
        private double _clientY;

        private List<IMenuItem> _menuItems = new List<IMenuItem>();


        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Id of HTML input. Optional.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public DateTime Value { get; set; }

        /// <summary>
        /// Tooltip for component.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime> ValueChanged { get; set; }

        /// <summary>
        /// Duplicate event call back for value changed. 
        /// It is necessary to have possibility catch changed even whet we use @bind-Value.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime> Changed { get; set; }

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
        public DropdownPositions DropdownPosition { get; set; } = DropdownPositions.Absolute;

        [Parameter]
        public FirstWeekDays FirstWeekDay { get; set; }

        [Parameter]
        public BootstrapElementSizes Size { get; set; }

        [Parameter]
        public string Format { get; set; } = "dd.MM.yyyy";

        [Parameter]
        public string HtmlStyle { get; set; }

        [Parameter]
        public string HtmlClass { get; set; }

        [Parameter]
        public int DropdownWidth { get; set; } = 210;

        [Parameter]
        public bool UseCustomMenu { get; set; }


        public string SizeClass => HtmlClassBuilder.BuildSizeClass("input-group", Size);

        public string DropdownTop
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

        public string DropdownLeft
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
                {
                    return $"{_inputElementInfo.LeftInt}px";
                }
                else
                {
                    return "";
                }

            }
        }

        public string DropdownWidthValue
        {
            get
            {
                return $"{DropdownWidth}px";
            }
        }

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

        public string DropdownMarginTop
        {
            get
            {
                if (DropdownPosition == DropdownPositions.Fixed)
                {
                    int topValue;
                    var dropHeight = 210;
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

                    if (_clientY > _windowHeight - dropHeight)
                    {
                        // Control is close to bottom. Open drop over the control.
                        topValue =  - dropHeight-topValue/2;
                    }

#if DEBUG
                    Console.WriteLine($"Dropdown top {topValue}px. {_clientY}, {_windowHeight}, {dropHeight}");
#endif

                    return $"{topValue}px";
                }

                return "0";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _culture = Thread.CurrentThread.CurrentCulture;

            _menuItems = new List<IMenuItem>();

            _menuItems.Add(new MenuItem()
            {
                Title = $"Clear",
                Name = $"clear",
                Kind = MenuItemKinds.Item,
                IconClass = "fa fa-times text-danger",
                HotKeyTooltip = "Alt+X"

            });

            _menuItems.Add(new MenuItem() { Kind = MenuItemKinds.Delimiter });

            _menuItems.Add(new MenuItem()
            {
                Title = $"Close",
                Name = $"close",
                Kind = MenuItemKinds.Item,
                IconClass = "fa fa-times text-secondary"
            });

            _inputGroupKey = "datepicker_" + Guid.NewGuid().ToString();
            try
            {
                await JsRuntime.InvokeAsync<object>("outsideClickHandler.addEvent", _inputGroupKey, DotNetObjectReference.Create(this));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DatePicker. Cannot register outside clicke handler. Message: {ex.Message}");
            }

        }

        protected override async Task OnParametersSetAsync()
        {

            // Fill day names
            if (FirstWeekDay == FirstWeekDays.Sunday)
            {
                _dayNames = _culture.DateTimeFormat.AbbreviatedDayNames;
            }
            else
            {
                var dayNamesTmp = _culture.DateTimeFormat.AbbreviatedDayNames;
                _dayNames = new string[7];

                for (var i = 1; i < 7; i++)
                {
                    _dayNames[i - 1] = dayNamesTmp[i];
                }
                _dayNames[6] = dayNamesTmp[0];

            }
            
            try
            {
                _windowHeight = await JsRuntime.InvokeAsync<double>("bbComponents.windowHeight");
                _windowWidth = await JsRuntime.InvokeAsync<double>("bbComponents.windowWidth");
            }
            catch (Exception e)
            {
                Console.WriteLine($"JS call error. Message: {e.Message}");
            }

            InitCalendar();


        }

        private async Task OnOpenClick(MouseEventArgs e)
        {
            _inputElementInfo = await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _inputElementReference);

            if (Value == DateTime.MinValue)
            {
                Value = DateTime.Now;
            }

            _clientX = e.ClientX;
            _clientY = e.ClientY;

#if DEBUG
            Console.WriteLine($"Date picker open click: {_clientX}, {_clientY}");
#endif

            _isOpen = !_isOpen;
            InitCalendar();

        }

        private async Task OnPreviousMonthClick()
        {
            if (Value == DateTime.MaxValue || Value == DateTime.MinValue)
            {
                Value = DateTime.Now;
            }

            Value = Value.AddMonths(-1);

            InitCalendar();

            await ValueChanged.InvokeAsync(Value);
            await Changed.InvokeAsync(Value);


        }

        private async Task OnNextMonthClick()
        {
            if (Value == DateTime.MaxValue || Value == DateTime.MinValue)
            {
                Value = DateTime.Now;
            }

            Value = Value.AddMonths(1);

            InitCalendar();

            await ValueChanged.InvokeAsync(Value);
            await Changed.InvokeAsync(Value);


        }

        private void InitCalendar()
        {
            if (Value == DateTime.MinValue)
            {
                return;
            }

            _year = Value.Year;
            _monthName = Value.ToString("MMMM");

            _stringValue = Value.ToString(Format, CultureInfo.InvariantCulture);

            _weeks = FillCalendarDays(Value, FirstWeekDay);

            _month = Value.Month;

        }

        private async Task OnDayClick(MouseEventArgs e, CalendarDay day)
        {
            Value = day.Date;
            _stringValue = Value.ToString(Format, CultureInfo.InvariantCulture);

            await ValueChanged.InvokeAsync(Value);
            await Changed.InvokeAsync(Value);

            _isOpen = false;

        }

        private async Task OnStringValueChange(ChangeEventArgs e)
        {
            if (IsDisabled)
            {
                return;
            }

            _stringValue = e.Value?.ToString();

            if (string.IsNullOrWhiteSpace(_stringValue))
            {
                await Clear();

                return;
            }

            if (DateTime.TryParseExact(_stringValue, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                Value = dateTime;
                _stringValue = Value.ToString(Format);

                await ValueChanged.InvokeAsync(Value);
                await Changed.InvokeAsync(Value);

            }
            else
            {
                _stringValue = Value.ToString(Format);
            }
        }

        private async Task OnInputKeyDown(KeyboardEventArgs e)
        {
           
            if (e.AltKey  && e.Code == "KeyX")
            {

                if (IsDisabled)
                {
                    return;
                }

                await Clear();

            }
          
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
          

        }

        [JSInvokable]
        public async Task InvokeClickOutside()
        {

            _isOpen = false;

            StateHasChanged();

        }

        private async Task Clear()
        {
            Value = DateTime.MinValue;
            _stringValue = "";
            await ValueChanged.InvokeAsync(Value);
            await Changed.InvokeAsync(Value);
        }

        public static DateTime GetFirstCalendarDate(DateTime value, FirstWeekDays firstWeekDay)
        {
            var startDay = new DateTime(value.Year, value.Month, 1);

            int shift;
            if (firstWeekDay == FirstWeekDays.Sunday)
            {
                shift = (int)startDay.DayOfWeek;
            }
            else
            {
                shift = startDay.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)startDay.DayOfWeek) - 1;
            }

            startDay = startDay.AddDays(-shift);

            return startDay;
        }

        public static List<List<CalendarDay>> FillCalendarDays(DateTime value, FirstWeekDays firstWeekDay)
        {
            var weeks = new List<List<CalendarDay>>();
            var startDay = GetFirstCalendarDate(value, firstWeekDay);

            var n = 0;
            for (var w = 1; w <= 6; w++)
            {

                var weekDays = new List<CalendarDay>();
                for (var d = 1; d <= 7; d++)
                {
                    var currentDay = startDay.AddDays(n);

                    var calendarDay = new CalendarDay() { Date = currentDay };
                    calendarDay.IsActive = calendarDay.Day == value.Day && calendarDay.Date.Month == value.Month;
                    calendarDay.IsAnotherMonth = calendarDay.Date.Month != value.Month;

                    weekDays.Add(calendarDay);

                    n++;
                }

                weeks.Add(weekDays);
            }

            return weeks;
        }
    }
}
