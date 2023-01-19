using System.Collections.Generic;
using System.Linq;
using BlazorServerSideForDebug.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorServerSideForDebug.Pages;

public partial class ListTestsPage: ComponentBase
{
    private List<Currency> _currencies;
    private Currency _selectedItem;
    
    protected override void OnInitialized()
    {
        _currencies = Currency.SampleData().ToList();

    }

    private void OnSetItemClick()
    {
         _selectedItem = _currencies[1];
    }

    private void OnItemClick(Currency item)
    {
        _selectedItem = item;
    }

}