using System;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Components;
using BlazorWebEngine.Interfaces;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeManager
    {
        public IElementServices ElementServices { get; init; }
        
        public NodeManager(IElementServices elementServices)
        {
            Console.WriteLine(elementServices.id);
            ElementServices = elementServices;
        }

        /// <summary>
        /// Automatically creates an instead of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GenerateItem<T>() where T : IInstance
        {
            IInstance item = (IInstance) Activator.CreateInstance(typeof(T), ElementServices);
            GenerateItem<T>(item);
            return (T)item;
        }

        /// <summary>
        /// If the item is a NodeBase type, .Instantiate() will be called on it.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        private void GenerateItem<T>(object item) where T: IInstance
        {
            if (!(item is NodeBase hold)) return;
            hold.Instantiate();
        }
    }
    
}