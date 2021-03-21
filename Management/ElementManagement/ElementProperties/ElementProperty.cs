using System;
using System.Collections.Generic;
using System.Diagnostics;
using BlazorWebEngine.Classes.Types.ActionCall;

namespace BlazorWebEngine.Management.ElementManagement.ElementProperties
{
    public class ElementProperty<T> where T : class
    {
        public Dictionary<string, ActionCall> PropertyChangedMap = new();

        public ActionCall<T, V> GetPropertyActionCall<V>()
        {
            var name = new StackTrace().GetFrame(1)?.GetMethod()?.Name.Split('_')[1];
            if (PropertyChangedMap.TryGetValue(name, out var hold))
                return hold as ActionCall<T, V>;

            var action = Activator.CreateInstance<ActionCall<T, V>>();
            PropertyChangedMap.Add(name, action);

            return action;
        }
    }
}