using System;
using System.Collections.Generic;
using BlazorWebEngine.Classes;

namespace BlazorWebEngine.Management.ElementHandling
{
    public class ElementInformation
    {
        /// <summary>
        /// ElementDataMap contains the composite objects of an element.
        /// </summary>
        public Dictionary<Type, Dictionary<int, object>> ElementDataMap = new();

        private T AddElementData<T>(int id, T data) where T: new()
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
        
        private T AddElementData<T>(int id) where T: new() => AddElementData<T>(id, new T());

        /// <summary>
        /// Retrieve data of a type from the ElementDataMap associated with the id of the element.
        /// </summary>
        /// <param name="elementBase"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetElementData<T>(ElementBase elementBase)  where T : new() => GetElementData<T>(elementBase.Id);
        
        /// <summary>
        /// Add default data of a type to the ElementDataMap associated with the id of the element.
        /// </summary>
        /// <param name="elementBase"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddElementData<T>(ElementBase elementBase) where T: new() => AddElementData<T>(elementBase.Id);
        
        /// <summary>
        /// Add data of a type to the ElementDataMap associated with the id of the element.
        /// </summary>
        /// <param name="elementBase"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddElementData<T>(ElementBase elementBase, T data) where T: new() => AddElementData<T>(elementBase.Id, data);
        

        private T GetElementData<T>(int id) where T: new()
        {
            Type type = typeof(T);
            if (!ElementDataMap.ContainsKey(type) || !ElementDataMap[type].ContainsKey(id))
            {
                return AddElementData<T>(id);
            }
            return (T) ElementDataMap[type][id];  
        } 
        
    }
}