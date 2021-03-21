using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management.NodeHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Components
{
    public class RootComponent
    {
        public RootComponent(NodeManager nodeManager)
        {
            RootItem = nodeManager.GenerateItem<CodedNode>();
        }

        public CodedNode RootItem { get; set; }

        public RenderFragment RenderNodes()
        {
            return b => { RootItem.RenderOutline(b); };
        }
    }
}