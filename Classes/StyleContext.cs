using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Classes
{
    public class StyleContext
    {
        public bool Valid = false;   
        private Dictionary<string, string> StyleMap { get; set; } = new();
        
        public StyleContext()
        {
        }
        
        private void AddStyle(StyleOperator styleOperator, string ElementId, string key, string value)
        {

            if (!StyleMap.ContainsKey(key))
            {               
                StyleMap.Add(key,value);
                
                if (!Valid) return;
                styleOperator.SetStyle(ElementId, (key, value));
            }
            else
            {
                if (StyleMap[key] != value && Valid)
                {
                    styleOperator.SetStyle(ElementId, (key, value));
                }
                
                StyleMap[key] = value;
            }
        }

        public void AddStyle(StyleOperator styleOperator, string ElementId, params (string, string)[] styles)
        {            

            foreach ((string key, string value) in styles)
            {
                AddStyle(styleOperator, ElementId, key, value);
            }
            CreateOutput();
        }

        private string Output { get; set; }
        
        public string GetRenderOutput()
        {
            Console.WriteLine("render");
            Valid = true;
            return Output;
        }
        
        private string CreateOutput()
        { 
            Output = string.Join(';', StyleMap.Select(e => $"{e.Key}:{e.Value}"));
            //Console.WriteLine(Output);
            return Output;
        }
        
    }
}