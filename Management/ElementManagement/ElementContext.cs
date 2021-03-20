using System;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Management.ElementManagement
{
    public class ElementContext : ElementProperties
    {
        public ElementContext(string id, Action<ElementProperties> action) : base(id)
        {
            Id = id;
            action(this);
        }

        public Action<RenderTreeBuilder> RenderElement { get; set; }
    }
}