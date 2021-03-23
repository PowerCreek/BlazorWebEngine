using System;
using System.Collections.Generic;
using BlazorWebEngine.Interfaces;

namespace BlazorWebEngine.Management.ElementManagement.ElementProperties
{
    public class ElementProperties
    {
        public Dictionary<string, IAttribute> AttributeMap = new();
        public Dictionary<string, object> ContextItemMap = new();

        public ElementProperties(string id)
        {
            Id = id;
        }

        public string Id { get; init; }

        public ElementProperties Add(string name, object data)
        {
            ContextItemMap.Add(name, data);
            return this;
        }

        public T Get<T>(string name)
        {
            return (T) ContextItemMap[name];
        }

        public ElementProperties Add<T>(string name, out T item)
        {
            ContextItemMap.Add(name, item = Activator.CreateInstance<T>());
            return this;
        }

        public ElementProperties AddAttribute<T>(string name, out T iAttribute) where T : IAttribute
        {
            AttributeMap.Add(name, iAttribute = Activator.CreateInstance<T>());
            return this;
        }

        public A GetAttribute<A>(string name) where A : IAttribute
        {
            return (A) AttributeMap[name];
        }

        public void RemoveAttribute(string name)
        {
            AttributeMap.Remove(name);
        }
    }
}