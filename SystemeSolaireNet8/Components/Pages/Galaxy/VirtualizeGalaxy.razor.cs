using Microsoft.AspNetCore.Components;
using SolarSystemN9.Data.Entity;
using SolarSystemN9.Data.Service;

namespace SolarSystemN9.Components.Pages.Galaxy;

public partial class VirtualizeGalaxy : ComponentBase
{
    public IQueryable<Bodies>? spaceEntities;

    [Inject]
    public SolarSystemService? SolarSystemService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (SolarSystemService == null)
        {
            throw new InvalidOperationException("SolarSystemService is not initialized.");
        }

        spaceEntities = await SolarSystemService.GetBodiesAsync();
    }
}