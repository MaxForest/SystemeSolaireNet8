using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SolarSystemN9.Data.Entity;
using SolarSystemN9.Data.Service;
using System.Reflection;

namespace SolarSystemN9.Components.Pages;

public partial class Details : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public string? Id { get; set; }

    public Bodies? SpaceEntity;

    [Inject]
    public SolarSystemService? SolarSystemService { get; set; }

    [Inject]
    public IJSRuntime? JS { get; set; }

    private IJSObjectReference? jsModule;

    protected PropertyInfo[]? spaceEntityProperties;

    protected override async Task OnInitializedAsync()
    {
        if (SolarSystemService == null)
        {
            throw new InvalidOperationException("SolarSystemService is not initialized.");
        }

        if (string.IsNullOrEmpty(Id))
        {
            throw new ArgumentException("Id parameter is required.");
        }

        SpaceEntity = await SolarSystemService.GetEntity(Id) ?? throw new Exception("Entity not found.");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (JS is not null)
            {
                jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/Details.razor.js");
            }
        }

        if (jsModule is not null && SpaceEntity is not null)
        {
            double pow = Math.Pow(10, 6);
            double semiminor = (SpaceEntity.SemimajorAxis * Math.Sqrt(1 - Math.Cbrt((double)SpaceEntity.Eccentricity))) / pow;
            double semimajor = SpaceEntity.SemimajorAxis / pow;

            //double aphelion = SpaceEntity.Aphelion / pow;
            //double perihelion = SpaceEntity.Perihelion / pow;

            await jsModule.InvokeVoidAsync("ellipse", 200, 200, semiminor, semimajor, Math.PI / 2, 0, 2 * Math.PI);
        }
    }

    protected override void OnParametersSet()
    {
        if (SpaceEntity != null)
        {
            spaceEntityProperties = SpaceEntity.GetType().GetProperties();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (jsModule is not null)
        {
            await jsModule.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}