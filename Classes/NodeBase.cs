using System;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Classes
{
    public class NodeBase : IInstance
    {

        public int Id { get; set; }
        public int Parent { get; set; }
        
        public OperationManager OperationManager;
        public NodeInformation NodeInformation;
        
        public NodeBase(int id, OperationManager operationManager, NodeInformation nodeInformation)
        {
            Id = id;
            OperationManager = operationManager;
            NodeInformation = nodeInformation;
        }
        
        public virtual void Instantiate()
        {
            Console.WriteLine("ElementBase");
            
        }
        
    }
}