using System;
using System.Threading.Tasks;
using BlazorWebEngine.Classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Components
{
    public partial class ContainerSpace : ComponentBase
    {

        [Inject] public Container ContainerData { get; set; }
         
        protected override void OnInitialized()
        {
            ContainerData.TriggerStateChange = StateHasChanged;
            base.OnInitialized();
        }
    }
}