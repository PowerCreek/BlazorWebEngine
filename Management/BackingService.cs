using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Management
{
    public class BackingService
    {
        public NodeInformation NodeInformation { get; init; }
        public NodeManager NodeManager { get; init; }
        public OperationManager OperationManager { get; init; }
        
        public BackingService(IBuilder backBuilder, NodeRegistry nodeRegistry, NodeInformation nodeInformation, 
            NodeManager nodeManager, OperationManager operationManager)
        {
            OperationManager = operationManager;
            NodeInformation = nodeInformation;
            NodeManager = nodeManager;
            
            backBuilder.Build(this);
        }
    }
}