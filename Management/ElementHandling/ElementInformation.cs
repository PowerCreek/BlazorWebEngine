using System;
using System.Collections.Generic;
using BlazorWebEngine.Classes;

namespace BlazorWebEngine.Management.ElementHandling
{
    public class ElementInformation
    {
        /// <summary>
        /// MapType
        /// </summary>
        public Dictionary<Type, Dictionary<int, object>> ElementDataMap = new();

        /// <summary>
        /// Add Data of a Type to the ElementDataMap associated with the id of the element.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddElementData<T>(int id, T data)
        {
            Type mapKey = typeof(T);
            if (!ElementDataMap.ContainsKey(mapKey))
            {
                ElementDataMap.Add(mapKey, new());
            }

            if (!ElementDataMap[mapKey].TryAdd(id, data))
            {
                ElementDataMap[mapKey][id] = data;
            }
            
            return data;
        }
        
        public T GetElementData<T>(int id)
        {
            
            return (T) ElementDataMap[typeof(T)][id];
        }
    }
}