using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using SolarSystemN9.Data.Entity;

namespace SolarSystemN9.Components.Table;

public partial class VirtualizeSpaceTable : ComponentBase
{
    private QuickGrid<Bodies>? grid;
    private GridItemsProvider<Bodies>? itemsProvider;
    private int numResults;
    private string? nameSearch;
    private CancellationTokenSource? debounceCts;

    [Parameter]
    public IQueryable<Bodies>? SpaceEntities { get; set; }

    protected override void OnInitialized()
    {
        itemsProvider = async request =>
        {
            GridItemsProviderResult<Bodies> result = await Task.Run(() => GetBodies(request, nameSearch));

            if (result.TotalItemCount != numResults && !request.CancellationToken.IsCancellationRequested)
            {
                numResults = result.TotalItemCount;
                StateHasChanged();
            }

            return result;
        };
    }

    private void DebounceSearch(ChangeEventArgs e)
    {
        debounceCts?.Cancel();
        debounceCts = new CancellationTokenSource();

        _ = Task.Delay(500, debounceCts.Token).ContinueWith(async task =>
        {
            if (!task.IsCanceled && grid != null)
            {
                await InvokeAsync(() => grid.RefreshDataAsync());
            }
        });
    }

    public GridItemsProviderResult<Bodies> GetBodies(GridItemsProviderRequest<Bodies> request, string? nameSearch)
    {
        IQueryable<Bodies>? bodies = SpaceEntities;

        if (!string.IsNullOrEmpty(nameSearch))
        {
            bodies = bodies?.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(nameSearch));
        }

        bodies = bodies?.OrderBy(x => x.Name)
                        .Skip(request.StartIndex)
                        .Take(request.Count ?? 20);

        return new GridItemsProviderResult<Bodies>
        {
            Items = bodies?.ToList() ?? [],
            TotalItemCount = SpaceEntities?.Count() ?? 0
        };
    }
}