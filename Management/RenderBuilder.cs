using System;
using BlazorWebEngine.Classes.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Management
{
    public class RenderBuilder
    {
        public int index;
        public RenderTreeBuilder RenderTreeBuilder;

        public RenderBuilder Open(RenderTreeBuilder renderTreeBuilder, string elementType)
        {
            RenderTreeBuilder = renderTreeBuilder;
            RenderTreeBuilder.OpenElement(index++, elementType);
            return this;
        }

        public RenderBuilder Open<C>(RenderTreeBuilder renderTreeBuilder) where C : ComponentBase
        {
            index = renderTreeBuilder.GetFrames().Count;
            RenderTreeBuilder = renderTreeBuilder;
            RenderTreeBuilder.OpenComponent<C>(index++);
            return this;
        }

        public RenderBuilder WithAttribute(string key, string value)
        {
            RenderTreeBuilder.AddAttribute(index++, key, value);
            return this;
        }

        public RenderBuilder WithAttribute(string key, object value)
        {
            RenderTreeBuilder.AddAttribute(index++, key, value);
            return this;
        }

        public RenderBuilder WithStyles(StyleContext styleContext)
        {
            RenderTreeBuilder.AddAttribute(index++, "style", styleContext.GetRenderOutput());
            return this;
        }

        public RenderBuilder WithReference(string key, object obj)
        {
            RenderTreeBuilder.AddAttribute(index++, key, obj);
            return this;
        }

        public RenderBuilder SetReferenceElement(Action<ElementReference> referral)
        {
            RenderTreeBuilder.AddElementReferenceCapture(index++, referral);
            return this;
        }

        public RenderBuilder WithAttribute(params (string key, string value)[] attr)
        {
            foreach ((var key, var value) in attr) WithAttribute(key, value);
            return this;
        }

        public RenderBuilder WithKey(string key)
        {
            RenderTreeBuilder.SetKey(key);
            return this;
        }

        public RenderBuilder WithContent(string content)
        {
            RenderTreeBuilder.AddContent(index++, content);
            return this;
        }

        public RenderBuilder WithMarkup(string markup)
        {
            RenderTreeBuilder.AddMarkupContent(index++, markup);
            return this;
        }

        public RenderBuilder End()
        {
            RenderTreeBuilder.CloseElement();
            return this;
        }

        public RenderBuilder End<C>()
        {
            RenderTreeBuilder.CloseComponent();
            return this;
        }
    }
}