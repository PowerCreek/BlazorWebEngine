using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebEngine.Classes
{
    public class ComponentStorage
    {
        public Dictionary<int, ComponentData> ComponentMap { get; } = new();
        
        public ComponentData AddComponent(int id, ComponentData componentData){
            ComponentMap.Add(id, Access(id, componentData));
            return componentData;
        }

        private void RemoveComponent(int id) => 
            ComponentMap.Remove(id);
        
        private ComponentData Access(int id, ComponentData componentData)
        {
            componentData.UnRegister = () => { RemoveComponent(id); };
            return componentData;
        }

        public ComponentData GetComponentByCondition(
            Func<ComponentData, bool> condition)
        {
            return ComponentMap.Values.First(condition);
        }
    }
}