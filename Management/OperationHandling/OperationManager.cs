using System;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Management.OperationHandling
{
    /// <summary>
    /// Rename this to not use 'Operator' in the name
    /// </summary>
    public class OperationManager
    {

        public Dictionary<Type, OperationBase> OperationMap = new();
        
        public OperationManager() {  }

        public T GetOperation<T>() where T: OperationBase
        {
            if (OperationMap.TryGetValue(typeof(T), out OperationBase obj))
            {
                return (T) obj;
            }

            T hold;
            OperationMap.Add(typeof(T),hold=Activator.CreateInstance<T>());
            hold.MakeOperator();
            return hold;
        }
        
    }
}