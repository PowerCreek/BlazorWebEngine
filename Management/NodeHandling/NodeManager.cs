using System;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeManager
    {
        public NodeRegistry NodeRegistry { get; set; }
        public NodeInformation NodeInformation { get; set; }
        
        public NodeManager(NodeRegistry nodeRegistry, NodeInformation nodeInformation)
        {
            NodeRegistry = nodeRegistry;
            NodeInformation = nodeInformation;
        }

        public T GenerateItem<T>(OperationManager operationManager) where T : IInstance
        {
            IInstance item = (IInstance) Activator.CreateInstance(typeof(T), NodeRegistry.AddNode(), operationManager, NodeInformation);
            GenerateItem<T>(item);
            return (T)item;
        }

        private void GenerateItem<T>(object t) where T: IInstance
        {
            if (!(t is NodeBase hold)) return;
            hold.Instantiate();
        }
    }
    
}