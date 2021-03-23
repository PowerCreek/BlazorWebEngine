using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Interfaces;
using BlazorWebEngine.Management.NodeHandling;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Management.ElementManagement
{
    public class ElementEventHandler
    {
        public Dictionary<string, Action<EventArgs>> EventMap { get; set; } = new();

        public void AddEvent(string key, Action<EventArgs> eventCallback)
        {
            if (!EventMap.TryAdd(key, eventCallback)) EventMap[key] += eventCallback;
        }
    }

    public class ElementContext : ElementProperties.ElementProperties
    {
        private static int _id;
        
        public Action<ElementContext, RenderBuilder> OnBuild = (_, _) => { };

        protected ElementContext(string id, NodeBase nodeBase) : base(id = $"{id}_{_id++}")
        {
            NodeBase = nodeBase;
        }

        public NodeBase NodeBase { get; init; }

        /// <summary>
        ///     The object that is created for the RenderTreeBuilder/Blazor dom construction.
        /// </summary>
        public StateComponent StateComponentReference { get; set; }

        /// <summary>
        ///     The sole element reference of this ElementContext.
        /// </summary>
        public ElementReference ElementReference { get; set; }

        /// <summary>
        ///     Contains a Dictionary housing the events that have been added outside of the class.
        /// </summary>
        public ElementEventHandler ElementEventHandler { get; init; } = new();

        public Dictionary<ElementContext, ElementContext> Children { get; set; } = new();

        /// <summary>
        ///     Builds the RenderFragment representing this ElementContext.
        /// </summary>
        /// <summary>
        ///     Builder.Open'StateComponent' is used within this method to reference the StateComponent class
        ///     in order to gather information from the references provided by this class as meta-component attributes
        /// </summary>
        public virtual Action<RenderBuilder, RenderTreeBuilder> BuildRenderFragment =>
            (builder, treeBuilder) =>
            {
                builder.Open<StateComponent>(treeBuilder);
                builder.WithAttribute("ElementContext", this);
                builder.WithAttribute("RenderBuilder", builder);
                treeBuilder.AddComponentReferenceCapture(builder.index++,
                    e => StateComponentReference = e as StateComponent);
                builder.End<StateComponent>();
            };

        public void AddChild(ElementContext childContext)
        {
            Children.TryAdd(childContext, childContext);
        }

        public void InitAction(Action<ElementContext> action)
        {
            action(this);
        }

        /// <summary>
        ///     Invoked within the StateComponent BuildRenderTree method.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="treeBuilder"></param>
        public virtual void InjectionBuild(RenderBuilder builder, RenderTreeBuilder treeBuilder)
        {
            builder.Open(treeBuilder, "div");
            builder.WithAttribute("id", Id);

            foreach (var (key, val) in ElementEventHandler.EventMap)
                builder.WithAttribute(key, EventCallback.Factory.Create(this, val));

            foreach (var (key, value) in AttributeMap)
                if (value is IRenderableItem item)
                    builder.WithAttribute(key, item?.GetRenderOutput());

            builder.SetReferenceElement(e => ElementReference = e);
            
            OnBuild(this, builder);

            Children.Keys.ToList().ForEach(
                e => e.BuildRenderFragment(builder, treeBuilder));

            builder.End();
        }
    }

    public class StateComponent : ComponentBase
    {
        [Parameter] public ElementContext ElementContext { get; set; }

        [Parameter] public RenderBuilder RenderBuilder { get; set; }

        public Action<StateComponent> DoAfterRender { get; set; } = component => { };
        
        public Action Reset => StateHasChanged;

        protected override void BuildRenderTree(RenderTreeBuilder renderTreeBuilder)
        {
            ElementContext.InjectionBuild(RenderBuilder, renderTreeBuilder);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            DoAfterRender?.Invoke(this);
        }
    }
}