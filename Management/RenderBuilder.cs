using System;
using BlazorWebEngine.Classes;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Management
{
    public class RenderBuilder
    {
        public int index = 0;
        public  RenderTreeBuilder RenderTreeBuilder;
        
        public RenderBuilder Open(RenderTreeBuilder renderTreeBuilder, string elementType)
        {
            RenderTreeBuilder = renderTreeBuilder;
            RenderTreeBuilder.OpenElement(index++, elementType);
            return this;
        }

        public RenderBuilder WithAttribute(string key, string value)
        {
            RenderTreeBuilder.AddAttribute(index++, key, value);
            return this;
        }

        public RenderBuilder WithStyles(StyleContext styleContext)
        {
            RenderTreeBuilder.AddAttribute(index++, "style", styleContext.GetRenderOutput());
            return this;
        }

        public RenderBuilder WithAttribute(params (string key, string value)[] attr)
        {
            foreach ((string key, string value) in attr)
            {
                WithAttribute(key, value);
            }
            return this;
        }
        
        public RenderBuilder WithKey(string key)
        {
            RenderTreeBuilder.SetKey(key);
            return this;
        }

        public RenderBuilder WithContent(string content)
        {
            RenderTreeBuilder.AddContent(index++,content);
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
    }
}