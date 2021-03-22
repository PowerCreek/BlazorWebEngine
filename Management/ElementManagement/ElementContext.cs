using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BlazorWebEngine.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.Management.ElementManagement
{

    public class ElementEventHandler
    {

        public Dictionary<string, Action<EventArgs>> EventMap { get; set; } = new();

        public void AddEvent(string key, Action<EventArgs> eventCallback)
        {
            if(!EventMap.TryAdd(key, eventCallback))
            {
                EventMap[key] += eventCallback;
            }
        }
    }
    
    public class ElementContext : ElementProperties.ElementProperties
    {

        public ElementEventHandler ElementEventHandler { get; init; } = new();
        public StateComponent StateComponentReference { get; set; }
        public ElementReference ElementReference { get; set; }
        public Dictionary<ElementContext,ElementContext> Children { get; set; } = new();
        public void AddChild(ElementContext childContext)=>Children.TryAdd(childContext,childContext);
        
        public ElementContext(string id, Action<ElementContext> action) : base(id)
        {
            Id = id;
            action(this);
        }

        public Action<ElementContext, RenderBuilder> OnBuild = (_, _) => { };
        
        public virtual Action<RenderBuilder, RenderTreeBuilder> BuildRenderFragment =>
            (builder, treeBuilder) =>
            {
                builder.Open<StateComponent>(treeBuilder);
                builder.WithAttribute("ElementContext", this);
                builder.WithAttribute("RenderBuilder", builder);
                treeBuilder.AddComponentReferenceCapture(builder.index++,
                    e=> StateComponentReference = e as StateComponent);
                builder.End<StateComponent>();
            };

        public virtual void InjectionBuild(RenderBuilder builder, RenderTreeBuilder treeBuilder)
        {
            builder.Open(treeBuilder, "div");
            builder.WithAttribute("id", Id);
            
            foreach (var (key,val) in ElementEventHandler.EventMap)
            {
                Console.WriteLine(key);
                builder.WithAttribute(key, EventCallback.Factory.Create(this,val));
            }
            
            OnBuild(this, builder);
            builder.SetReferenceElement(e => ElementReference = e);
            
            Children.Keys.ToList().ForEach(
                e => e.BuildRenderFragment(builder, treeBuilder));
            
            builder.End();
        }
    }

    public class StateComponent : ComponentBase
    {
        [Parameter]
        public ElementContext ElementContext { get; set; }
        
        [Parameter]
        public RenderBuilder RenderBuilder { get; set; }
        public Action Reset => StateHasChanged;

        protected override void BuildRenderTree(RenderTreeBuilder renderTreeBuilder)
        {
            ElementContext.InjectionBuild(RenderBuilder,renderTreeBuilder);
        }
    } 
}