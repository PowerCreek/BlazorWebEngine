using System;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Management.ElementManagement
{
    public class ElementContext : ElementProperties.ElementProperties
    {
        public ElementContext(string id, Action<ElementProperties.ElementProperties> action) : base(id)
        {
            Id = id;
            action(this);
        }

        public Action<RenderTreeBuilder> RenderElement { get; set; }
    }
}