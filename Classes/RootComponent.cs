using System;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Classes
{
    public class RootComponent
    {
        public CodedNode RootItem { get; set; }
        
        public RootComponent(NodeManager nodeManager, OperationManager operationManager)
        {
            Init(nodeManager, operationManager);
        }

        public void Init(NodeManager nodeManager, OperationManager operationManager)
        {
            RootItem = nodeManager.GenerateItem<CodedNode>(operationManager);            

        }
        
        public RenderFragment RenderNodes()
        {
            return b =>
            {
                RootItem.RenderOutline(b);
            };
        }

    }
}