using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using SystemeSolaireNet8.Data.Entity;

namespace SystemeSolaireNet8.Components.Table
{
    public class SpaceBase : ComponentBase
  {
    [Parameter]
    public required IQueryable<SpaceEntity> SpaceEntities { get; set; }

    public async Task<GridItemsProviderResult<SpaceEntity>> GetBodies(GridItemsProviderRequest<SpaceEntity> request, string? nameSearch)
    {
      var bodies = SpaceEntities;

      if (!string.IsNullOrEmpty(nameSearch))
      {
        bodies = bodies.Where(x => x.name.Contains(nameSearch));
      }

      bodies =  bodies.OrderBy(x => x.name)
                      .Skip(request.StartIndex)
                      .Take(request.Count ?? 20);
      
      if (bodies != null)
      {
        return new GridItemsProviderResult<SpaceEntity>()
        {
          Items = bodies.ToArray(),
          TotalItemCount = SpaceEntities.Count()
        };
      }

      throw new Exception();
    }
  }
}
