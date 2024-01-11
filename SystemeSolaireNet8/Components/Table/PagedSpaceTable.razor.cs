using Microsoft.AspNetCore.Components;
using SystemeSolaireNet8.Data.Entity;

namespace SystemeSolaireNet8.Components.Table
{
    public class PagedSpaceBase : ComponentBase
  {
    [Parameter]
    public required IQueryable<SpaceEntity> SpaceEntities { get; set; }
  }
}
