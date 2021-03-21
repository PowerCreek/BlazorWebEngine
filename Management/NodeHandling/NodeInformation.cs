using System;
using System.Collections.Generic;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeInformation
    {
        /// <summary>
        ///     ElementDataMap contains the composite objects of an element.
        /// </summary>
        public Dictionary<Type, Dictionary<int, object>> NodeDataMap = new();

        public T AddNodeData<T>(NodeBase node)
        {
            return AddNodeData(node, (T) Activator.CreateInstance(typeof(T), node));
        }

        private T AddNodeData<T>(NodeBase elem, T data)
        {
            var mapKey = typeof(T);
            if (!NodeDataMap.ContainsKey(mapKey)) NodeDataMap.Add(mapKey, new());

            if (!NodeDataMap[mapKey].TryAdd(elem.Id, data)) NodeDataMap[mapKey][elem.Id] = data;
            return data;
        }

        /// <summary>
        ///     Retrieve data of a type from the ElementDataMap associated with the id of the element.
        /// </summary>
        /// <param name="nodeBase"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetNodeData<T>(NodeBase nodeBase)
        {
            if (NodeDataMap.TryGetValue(typeof(T), out var map)) return (T) map[nodeBase.Id];
            return AddNodeData<T>(nodeBase);
        }
    }
}