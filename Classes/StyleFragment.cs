using System;
using System.Collections.Generic;
using BlazorWebEngine.Components;
using BlazorWebEngine.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Classes
{
    public class StyleFragment
    {
        public bool IsValid { get; set; } = false;
        public string StyleKey { get; set; }
        public string Selector { get; init; }

        public string HtmlContent;
        private List<(string, string)> _htmlContentList = new ();
        public List<(string, string)> HtmlContentList
        {
            get
            {
                IsValid = false;
                return _htmlContentList;
            }
            set { _htmlContentList = value; }
        }

        public string HtmlRender()
        {
            if (!IsValid)
            {
                HtmlContent = BuildStyleItems(HtmlContentList);
            }

            IsValid = true;
            return HtmlContent;
        }

        private string BuildStyleItems(IEnumerable<(string style, string value)> contentList)
        {
            string styleData = $"{Selector}";
            styleData += "{";

            foreach (var (style, value) in contentList)
            {
                styleData += $"{style}: {value};";
            }

            styleData += "}";
            Console.WriteLine(styleData);
            return styleData;
        }

        public PremadeObjects.StyleItemTemplate GetStyleElement => PremadeObjects.GetStyleItem(this);


        public Action TriggerStateChange { get; set; } = () => { };
    }
}