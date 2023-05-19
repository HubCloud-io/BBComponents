using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorServerSideForDebug.Pages;

public partial class AutoCompleteTests:ComponentBase
{
    private List<string> _fieldSource = new List<string>();
    private string _field1 = "$h.id";
    private string _field2;


    protected override void OnInitialized()
    {
        _fieldSource.Add("$h.id");
        _fieldSource.Add("$h.date");
        _fieldSource.Add("$h.number");
        
        _fieldSource.Add("$r.product");
        _fieldSource.Add("$r.quantity");
        _fieldSource.Add("$r.price");
        _fieldSource.Add("$r.amount");

    }
}