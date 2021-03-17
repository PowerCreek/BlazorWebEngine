using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Dynamic;
using System.Threading.Tasks.Sources;
using BlazorWebEngine.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.Classes
{
    public class ElementDefinition
    {
        public ElementDefinition(string id) => Id = id;
        
        public List<string> PreventEvents { get; set; } = new();
        public List<string> StopPropagation { get; set; } = new();
        public List<(string, Action<EventArgs>)> EventListeners { get; set; } = new();
        public List<(string, string)> AttributeList { get; } = new();

        public string Id { get; private init; }

        public string Key { get; set; } = "";

        private RenderFragment Fragment { get; set; }

        public Action<RenderTreeBuilder> FragmentConstruction = _ => { };

        public RenderFragment GetFragment()
        {
            if (Fragment != null) return Fragment;
            return Fragment = b => FragmentConstruction(b);
        }

        public RenderFragment Render => GetFragment();

        private int ElementIndex;
        
        private ElementDefinition AttachBuilder(Action<RenderTreeBuilder> action)
        {
            FragmentConstruction += action;
            return this;
        }
        
        public ElementDefinition OpenElement(string elem) => AttachBuilder(builder => builder.OpenElement(ElementIndex++, elem));

        public ElementDefinition WithId(string customId) => AttachBuilder(builder =>
        {
            builder.AddAttribute(ElementIndex++, "id", customId);
            builder.SetKey(customId);
        });

        public ElementDefinition WithAttributes(params (string, object)[] attributes) => AttachBuilder(builder =>
        {
            foreach (var (item1, item2) in attributes)
            {
                builder.AddAttribute(ElementIndex++, item1, item2);
            }
        });

        public ElementDefinition WithPreventDefaults(params string[] preventEvents) => AttachBuilder(builder =>
        {
            foreach (var eventName in preventEvents)
            {
                builder.AddEventPreventDefaultAttribute(ElementIndex++, eventName, true);
            }
        });

        public ElementDefinition WithStopPropagations(params string[] propagations) => AttachBuilder(builder =>
        {
            foreach (var eventName in propagations)
            {
                builder.AddEventStopPropagationAttribute(ElementIndex++, eventName, true);
            }
        });

        public ElementDefinition WithEventCallbacks(params (string, Action<EventArgs>)[] events) => AttachBuilder(builder =>
        {
            foreach (var (item1, action) in events)
            {
                builder.AddAttribute(ElementIndex++, item1, EventCallback.Factory.Create(this, action));
            }
        });

        public ElementDefinition CloseElement() => AttachBuilder(builder => builder.CloseElement());
    }
}