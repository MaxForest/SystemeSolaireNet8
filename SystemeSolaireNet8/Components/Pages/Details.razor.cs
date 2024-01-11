using Microsoft.AspNetCore.Components;
using SystemeSolaireNet8.Data.Entity;
using SystemeSolaireNet8.Data.Service;

namespace SystemeSolaireNet8.Components.Pages
{
    public class DetailsComponent : ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }

        public SpaceEntity? SpaceEntity;

        [Inject]
        public SolarSystemService SolarSystemService { get; set; }

        protected override async Task OnInitializedAsync()
        {
          // Simulate asynchronous loading to demonstrate streaming rendering
          await Task.Delay(500);
          SpaceEntity = !string.IsNullOrEmpty(Id) ? await SolarSystemService.GetEntity(Id) : throw new Exception();
        }
    }
}
