﻿using System;
using System.Collections.Generic;

namespace BlazorWebEngine.Management.ElementManagement.ElementProperties
{
    public class ElementProperties
    {
        public Dictionary<string, object> ContextItemMap = new();

        public ElementProperties(string id)
        {
            Id = id;
        }

        public string Id { get; init; }

        public T Get<T>(string name)
        {
            return (T) ContextItemMap[name];
        }

        public ElementProperties Add<T>(string name, out T item)
        {
            ContextItemMap.Add(name, item = Activator.CreateInstance<T>());
            return this;
        }

        public ElementProperties Add(string name, object data)
        {
            ContextItemMap.Add(name, data);
            return this;
        }
    }
}