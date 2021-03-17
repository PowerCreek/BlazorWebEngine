using System.Collections.Generic;
using System.Data.Common;

namespace BlazorWebEngine.Classes
{
    public struct RegistrationData
    {
        public int Id { get; set; }
        public StyleRef StyleRef { get; set; }
        public ComponentData ComponentData { get; set; }
        
        public RegistrationData(int id, string selector, StyleRef styleRef, ComponentData componentData)
        {
            Id = id;
            StyleRef = styleRef;
            ComponentData = componentData;
            ComponentData.Selector = $"{selector}";
            
        }
    }
}