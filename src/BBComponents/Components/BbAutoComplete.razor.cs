using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BBComponents.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BBComponents.Components;

public partial class BbAutoComplete:ComponentBase
{
    private const int DefaultDropdownWidth = 250;

    
    private string _value;
    private bool _isOpen;
    
    private ElementReference _inputElementReference;
    private HtmlElementInfo _inputElementInfo;
    
    private double _windowWidth;
    private double _windowHeight;

    
    /// <summary>
    /// Id of HTML input. Optional.
    /// </summary>
    [Parameter]
    public string Id { get; set; }

    /// <summary>
    /// Classes for input. Optional.
    /// </summary>
    [Parameter]
    public string HtmlClass { get; set; }

    /// <summary>
    /// Style for HTML input. Optional.
    /// </summary>
    [Parameter]
    public string HtmlStyle { get; set; }

    /// <summary>
    /// Indicates that component is read-only.
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Indicates that component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Tooltip for component.
    /// </summary>
    [Parameter]
    public string Tooltip { get; set; }

    /// <summary>
    /// Placeholder for input.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; }
    
    [Parameter]
    public int DropdownWidth { get; set; } = DefaultDropdownWidth;
    
    /// <summary>
    /// Colllection for select options.
    /// </summary>
    [Parameter]
    public IEnumerable<string> ItemsSource { get; set; }
    
    /// <summary>
    /// String value.
    /// </summary>
    [Parameter]
    public string Value { get; set; }

    /// <summary>
    /// Event call back for value changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// <summary>
    /// Duplicate event call back for value changed. 
    /// It is necessary to have possibility catch changed even whet we use @bind-Value.
    /// </summary>
    [Parameter]
    public EventCallback<string> Changed { get; set; }
    
    [Inject]
    public IJSRuntime JsRuntime { get; set; }

    public List<string> SourceFiltered
    {
        get
        {
            if (string.IsNullOrEmpty(_value))
            {
                return ItemsSource.ToList();
            }

            var sourceFiltered = ItemsSource
                .Where(x => x.Equals(_value, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return sourceFiltered;
        }
    }
    
    public string DropdownTop
    {
        get
        {
                return $"{_inputElementInfo.TopInt}px";
         
        }
    }
    
    public string DropdownMarginTop
    {
        get
        {
                int topValue;
                var dropHeight = 210;
                topValue = 39;

                if (_inputElementInfo.Top > _windowHeight - dropHeight)
                {
                    // Control is close to bottom. Open drop over the control.
                    topValue = -dropHeight - topValue / 2;
                }

                return $"{topValue}px";
            
        }
    }
    
    public string DropdownWidthValue
    {
        get
        {
                var width = DropdownWidth > 0 ? DropdownWidth : DefaultDropdownWidth;

                return $"{width}px";

        }
    }

    public string DropdownLeft
    {
        get
        {
                return $"{_inputElementInfo.LeftInt}px";

        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _windowHeight = await JsRuntime.InvokeAsync<double>("bbComponents.windowHeight");
            _windowWidth = await JsRuntime.InvokeAsync<double>("bbComponents.windowWidth");
        }
        catch (Exception e)
        {
            Console.WriteLine($@"JS call error bbComponents.windowHeight. Message: {e.Message}");
        }
    }

    private async Task OnValueChange(ChangeEventArgs e)
    {
        _value = e.Value?.ToString();

        await ValueChanged.InvokeAsync(_value);
        await Changed.InvokeAsync(_value);

        if (SourceFiltered.Any())
        {
            try
            {
                _inputElementInfo = await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _inputElementReference);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"JS call error getElementInfo. Message: {ex.Message}");
            }
            
            _isOpen = true;
        }

    }

    private async Task OnItemClick(string value)
    {
        _value = value;
        _isOpen = false;
        
        await ValueChanged.InvokeAsync(_value);
        await Changed.InvokeAsync(_value);
    }

}