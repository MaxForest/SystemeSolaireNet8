using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using SolarSystemN9.Data.Entity;

namespace SolarSystemN9.Components.Table;

public partial class VirtualizeSpaceTable : ComponentBase
{
    private QuickGrid<CelestialBody>? grid;
    private GridItemsProvider<CelestialBody>? itemsProvider;
    private int numResults;
    private string? nameSearch;
    private CancellationTokenSource? debounceCts;

    [Parameter]
    public IQueryable<CelestialBody>? CelestialBodies { get; set; }

    protected override void OnInitialized()
    {
        itemsProvider = async request =>
        {
            GridItemsProviderResult<CelestialBody> result = await Task.Run(() => GetBodies(request, nameSearch));

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

    public GridItemsProviderResult<CelestialBody> GetBodies(GridItemsProviderRequest<CelestialBody> request, string? nameSearch)
    {
        IQueryable<CelestialBody>? bodies = CelestialBodies;

        if (!string.IsNullOrEmpty(nameSearch))
        {
            bodies = bodies?.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(nameSearch));
        }

        bodies = bodies?.OrderBy(x => x.Name)
                        .Skip(request.StartIndex)
                        .Take(request.Count ?? 20);

        return new GridItemsProviderResult<CelestialBody>
        {
            Items = bodies?.ToList() ?? [],
            TotalItemCount = CelestialBodies?.Count() ?? 0
        };
    }
}