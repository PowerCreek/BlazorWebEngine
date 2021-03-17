using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Classes
{
    
    public class ComponentData
    {
        public ComponentData(int id) => ComponentId = id;

        public readonly Dictionary<int, ComponentData> Children = new();
            
        public int ComponentId { get; set; }

        public string Selector { get; set; }
        
        public ElementDefinition ComponentDef { get; set; }
        
        public string Parent { get; set; }
        
        public Action UnRegister = () => { };

        public Transform Transform { get; init; } = new Transform();
        
        public Action<string> DoSetParent = _ => { };

        public void SetParent(string parent) => DoSetParent(parent);

        public void SetParent(ComponentData parent) => SetParent(parent.Selector);
    }
}