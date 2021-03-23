using System.Collections.Generic;
using BlazorWebEngine.Management.ElementManagement;

namespace BlazorWebEngine.Components
{
    public class ElementContextProvider
    {
        public Dictionary<string, ElementContext> ContextMap { get; set; } = new();

        public ElementContext AddContext(ElementContext context)
        {
            ContextMap.Add(context.Id, context);
            return context;
        }

        public ElementContext GetContext(string id)
        {
            return ContextMap[id];
        }

        public T GetElementProperty<T>(string id, string propName)
        {
            return GetContext(id).Get<T>(propName);
        }
    }
}