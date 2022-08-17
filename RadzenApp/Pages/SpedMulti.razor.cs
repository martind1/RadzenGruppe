using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using RadzenApp.Shared;
using RadzenApp.Data;
using RadzenApp.Entities.QUVA;
using RadzenApp.Services;

namespace RadzenApp.Pages;

public partial class SpedComponent : ComponentBase
{
    [Inject]
    public QuvaData Data { get; set; }

    protected RadzenDataGrid<SPEDITIONEN> grid;
    protected IEnumerable<SPEDITIONEN> query;
    protected int count;
    protected bool isLoading = false;
    protected bool allowFiltering = false;
    private String oldArgs = "-1";

    protected String EqualsText = "Gleich";
    protected String ApplyFilterText = "Anwenden";
    protected String ClearFilterText = "Zurücksetzen";
    protected String NotEqualsText = "Ungleich";

    protected EventConsole? console;

    [Inject]
    private GlobalService GNav { get; set; }

    protected async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;
        await Task.Yield();

        //08.08.22 mit Radzen 3.20.2 nicht mehr notwendig
        //if (args.Top == 9)
        //{
        //    GNav.SMessL($"Skip: {args.Skip}, Top: {args.Top} Ignored", console);
        //    return;
        //}
        GNav.SMessL($"Skip: {args.Skip}, Top: {args.Top}", console);

        query = Data.SpedQuery(args);
        if (oldArgs != args.Filter)
        {
            oldArgs = args.Filter;
            count = Data.SpedQueryCount(args);
            GNav.SMessL($"Loaded. Count: {count}", console);
        }

        //if (args.Top > 20)
            isLoading = false;
    }

    #region Kommando von GlobalNavigator über GlobalService nach hier
    protected override void OnInitialized()
    {
        GNav.SetGrid<SPEDITIONEN>(grid);
        GNav.OnDoKommando += RunKommando;
    }

    public void RunKommando(int KNr)
    {
        switch ((GlobalService.KommandoTyp)KNr)
        {
            case GlobalService.KommandoTyp.Suchen:
                {
                    allowFiltering = !allowFiltering;
                    //if (allowFiltering)
                    //    _ = InsertRow();
                    StateHasChanged();
                    break;
                }
            case GlobalService.KommandoTyp.Refresh:
                {
                    _ = Reset();
                    break;
                }
        }
    }

    async Task Reset()
    {
        //grid.Reset(true);
        await grid.Reload();
        //await grid.FirstPage(true);
    }

    #endregion

    #region Suchen per InsertRow

    SPEDITIONEN? recordToInsert;

    private async Task InsertRow()
    {
        recordToInsert = new SPEDITIONEN();
        await grid.InsertRow(recordToInsert);

    }


    #endregion
}
