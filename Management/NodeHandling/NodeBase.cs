using System;
using BlazorWebEngine.Interfaces;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeBase : IInstance
    {
        public NodeBase(IElementServices elementServices)
        {
            Id = elementServices.NodeRegistry.AddNode();
            ElementServices = elementServices;
        }

        public int Id { get; set; }
        public int Parent { get; set; }
        public IElementServices ElementServices { get; init; }

        public virtual void Instantiate()
        {
            Console.WriteLine("ElementBase");
        }
    }
}