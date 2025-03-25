using Microsoft.AspNetCore.Components;
using SolarSystemN9.Data.Entity;

namespace SolarSystemN9.Components.Card
{
    public partial class SpaceCard
    {
        [Parameter, EditorRequired]
        public Bodies Body { get; set; } = default!;
    }
}