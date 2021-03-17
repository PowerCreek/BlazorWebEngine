using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlazorWebEngine.Classes
{
    public class StyleRef
    {

        public readonly int StyleRefId = Generator.NextIndex;
        public delegate void StyleAction(int index, string selector, string styleKey, params (string Style, string Value)[] styles);
        public StyleAction PerformStyleAction;
        
        public StyleRef(StyleAction styleAction)
        {
            PerformStyleAction = styleAction;
        }

        //selector, stylekey
        private Dictionary<string, List<(string, string)>> StyleMap { get; } = new();

        public Dictionary<string, StyleFragment> SelectorMap { get; } = new();
        
        public void ApplyStyles(string selector, string styleKey, params (string Style, string Value)[] styles)
        {
            string fragKey = $"{selector}_{styleKey}";

            if (!StyleMap.ContainsKey(fragKey))
            {
                StyleMap.Add(fragKey, new List<(string, string)>(styles));
            }
            else
            {
                StyleMap[fragKey] = new List<(string, string)>(styles);
            }

            if (!SelectorMap.ContainsKey(selector))
            {
                SelectorMap.Add(selector, new StyleFragment
                {
                      Selector = selector,
                      StyleKey = styleKey
                });
            }
            else
            {
                SelectorMap[selector].HtmlContentList.Clear();
            }

            foreach (var (key, value) in StyleMap)
            {
                if (key.StartsWith(selector))
                {
                    SelectorMap[selector].HtmlContentList.AddRange(value);
                }
            }
        }

        public StyleRef StyleCallback(ComponentData componentData, string styleKey, params (string Style, string Value)[] styles)
        {
            PerformStyleAction(componentData.ComponentId, componentData.Selector, styleKey, styles);
            return this;
        }
        
        public StyleRef StyleCallback(int id, string selector, string styleKey, params (string Style, string Value)[] styles)
        {
            
            PerformStyleAction(id, selector, styleKey, styles);
            return this;
        }
    }
}