using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Classes.Contexts
{
    public class StyleContext
    {
        public bool Valid;

        public Dictionary<string, string> StyleMap { get; } = new();

        private string Output { get; set; }

        public void AddStyle(StyleOperator styleOperator, string ElementId, params (string, string)[] styles)
        {
            foreach (var (key, value) in styles) AddStyle(styleOperator, ElementId, key, value);
            CreateOutput();
        }
        
        private void AddStyle(StyleOperator styleOperator, string ElementId, string key, string value)
        {
            if (!StyleMap.ContainsKey(key))
            {
                StyleMap.Add(key, value);

                if (!Valid) return;
                styleOperator.SetStyle(ElementId, (key, value));
            }
            else
            {
                if (StyleMap[key] != value && Valid) styleOperator.SetStyle(ElementId, (key, value));

                StyleMap[key] = value;
            }
        }

        public void AddStyle(StyleOperator styleOperator, ElementContext elementContext, params (string, string)[] styles)
        {
            foreach (var (key, value) in styles) AddStyle(styleOperator, elementContext, key, value);
            CreateOutput();
        }
        
        private void AddStyle(StyleOperator styleOperator, ElementContext elementContext, string key, string value)
        {
            if (!StyleMap.ContainsKey(key))
            {
                StyleMap.Add(key, value);

                if (Equals(default,elementContext.ElementReference) || !Valid) return;
                styleOperator.SetStyle(elementContext.ElementReference, key, value);
            }
            else
            {
                if (!Equals(default,elementContext.ElementReference) && StyleMap[key] != value && Valid) 
                    styleOperator.SetStyle(elementContext.ElementReference, key, value);

                StyleMap[key] = value;
            }
        }

        public string GetRenderOutput()
        {
            Console.WriteLine("render");
            Valid = true;
            return Output;
        }

        public string CreateOutput()
        {
            Output = string.Join(';', StyleMap.Select(e => $"{e.Key}: {e.Value}"));
            return Output;
        }
    }
}