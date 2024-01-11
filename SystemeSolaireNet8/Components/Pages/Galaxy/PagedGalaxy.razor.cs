using Microsoft.AspNetCore.Components;
using SystemeSolaireNet8.Data.Entity;
using SystemeSolaireNet8.Data.Service;

namespace SystemeSolaireNet8.Pages;

public class PagedGalaxyComponent : ComponentBase
{
  public IQueryable<SpaceEntity>? spaceEntities;

  [Inject]
  public SolarSystemService SolarSystemService { get; set; }

  protected override async Task OnInitializedAsync()
  {
    spaceEntities = await SolarSystemService.GetBodies();
  }
}
