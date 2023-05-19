using BBComponents.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Components;

/// <summary>
/// Represents a button component styled with Bootstrap.
/// </summary>
public partial class BbButton: ComponentBase
{

    private bool _isClicking;

    /// <summary>
    /// Gets or sets the Id of the button element.
    /// </summary>
    [Parameter]
    public string Id { get; set; }
    
    /// <summary>
    /// Classes for HTML element. Optional.
    /// </summary>
    [Parameter]
    public string HtmlClass { get; set; }

    /// <summary>
    /// Style for HTML element. Optional.
    /// </summary>
    [Parameter]
    public string HtmlStyle { get; set; }

    /// <summary>
    /// Gets or sets the Bootstrap color scheme for the button.
    /// </summary>
    [Parameter]
    public BootstrapColors Color { get; set; }

    /// <summary>
    /// Gets or sets the size of the button based on Bootstrap button sizes.
    /// </summary>
    [Parameter] public BootstrapButtonSizes Size { get; set; } = BootstrapButtonSizes.Sm;

    /// <summary>
    /// Gets or sets the Bootstrap style for the button.
    /// </summary>
    [Parameter] public BootstrapButtonStyles ButtonStyle { get; set; } = BootstrapButtonStyles.Outline;

    /// <summary>
    /// Gets or sets the text displayed on the button.
    /// </summary>
    [Parameter]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for the button's icon.
    /// </summary>
    [Parameter]
    public string IconClass { get; set; }
    
    /// <summary>
    /// Indicates that component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is in a waiting state.
    /// </summary>
    [Parameter]
    public bool IsWaiting { get; set; }

    /// <summary>
    /// Child content of the button, if any.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Event callback for the click event of the button.
    /// </summary>
    [Parameter]
    public EventCallback Clicked { get; set; }

    private async Task OnClicked()
    {
        if(_isClicking)
            return;

        _isClicking = true;
        
        await Clicked.InvokeAsync(null);

        _isClicking = false;
    }

    /// <summary>
    /// Generates a string of CSS classes for the button based on the set parameters.
    /// </summary>
    private string ComponentClass()
    {
        var sb = new StringBuilder();
        sb.Append("bb-button btn ");

        if (Size != BootstrapButtonSizes.Default)
        {
            sb.Append("btn-").Append(Size.ToString().ToLower()).Append(' ');
        }

        var color = Color.ToString().ToLower();

        if (ButtonStyle.HasFlag(BootstrapButtonStyles.Outline))
            sb.Append("btn-outline-").Append(color).Append(' ');
        else
            sb.Append("btn-").Append(color).Append(' ');

        if (!string.IsNullOrWhiteSpace(HtmlClass))
        {
            sb.Append(HtmlClass);
        }

        return sb.ToString().Trim();
        
    }
}