﻿using System.Collections.Generic;

namespace BlazorWebEngine.Components
{
    public class ComponentMap
    {
        public ComponentMap()
        {
            IdMap = new();
        }

        public Dictionary<string, object> IdMap { get; init; }

        public T GetComponent<T>(string id)
        {
            return (T) IdMap[id];
        }

        public void AddComponent(string id, object item)
        {
            if (IdMap.ContainsKey(id))
                IdMap.Add(id, item);
            else
                IdMap[id] = item;
        }
    }
}