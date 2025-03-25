using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using SolarSystemN9.Data.Entity;

namespace SolarSystemN9.Components.Table
{
    public partial class PagedSpaceTable : ComponentBase
    {
        [Parameter]
        public IQueryable<Bodies>? SpaceEntities { get; set; }

        private readonly PaginationState pagination = new() { ItemsPerPage = 15 };
        private string nameSearch = string.Empty;

        private readonly GridSort<Bodies> rankSort = GridSort<Bodies>
            .ByDescending(x => x.MeanRadius)
            .ThenDescending(x => x.Density);

        private IQueryable<Bodies>? FilteredItems => SpaceEntities?.Where(x => !string.IsNullOrEmpty(x.Name)
                                                                               && x.Name.Contains(nameSearch, StringComparison.CurrentCultureIgnoreCase));
    }
}