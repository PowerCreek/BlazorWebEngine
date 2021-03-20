using System;
using System.Threading.Tasks;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Components
{

    public class TestComp : Basic
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int index = builder.GetFrames().Count;
            builder.OpenElement(index++, "div");
            builder.AddAttribute(index++,"id",id);
            builder.AddAttribute(index++, "style", Context.Get<StyleContext>("StyleContext").GetRenderOutput());
            builder.CloseElement();
            base.BuildRenderTree(builder);
        }
    }
    
    public class Basic : ComponentBase
    {
        [Parameter]
        public string id { get; set; }
        
        [Inject]
        public ElementContextProvider ElementContextProvider { get; set; }
        
        [Inject]
        public ComponentMap ComponentMap { get; set; }

        public ElementContext Context => ElementContextProvider.GetContext(id);
        
        public Action TriggerStateChanged => StateHasChanged;

        protected override void OnInitialized() => ComponentMap.AddComponent(id, this);

    }
}