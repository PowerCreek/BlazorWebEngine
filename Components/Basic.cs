using System;
using System.Dynamic;
using System.Threading.Tasks;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management.ElementManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.Components
{
    public class TestElement : Basic
    {
        
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var index = 0;
            builder.OpenElement(index++, "div");
            builder.AddAttribute(index++, "id", id);
            builder.AddAttribute(index++, "onclick", EventCallback.Factory.Create(this, OnClick));
            builder.AddAttribute(index++, "style", Context.Get<StyleContext>("StyleContext").GetRenderOutput());
            builder.CloseElement();
            base.BuildRenderTree(builder);
        }
        
    }

    public class Basic : ComponentBase
    {
        
        [Parameter] public Action<MouseEventArgs> OnClick { get; set; }
        
        [Parameter] public string id { get; set; }

        [Inject] public ElementContextProvider ElementContextProvider { get; set; }

        [Inject] public ComponentMap ComponentMap { get; set; }

        public ElementContext Context => ElementContextProvider.GetContext(id);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
        }

        protected override void OnInitialized()
        {
            ComponentMap.AddComponent(id, this);
        }
    }
}