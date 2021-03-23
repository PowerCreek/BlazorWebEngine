using System;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class BodyContextExample : ElementContext
    {
        public ContainerContext Container;
        public BodyContextExample(NodeBase nodeBase) : base($"Body{nodeBase.Id}", nodeBase)
        {
            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "absolute"),
                ("width", "100%"),
                ("height", "100%"),
                ("background-color", "blue")
            );
        }

        public BodyContextExample(NodeBase nodeBase, out ElementContext self) : this(nodeBase)
        {
            self = this;

            Container = new ContainerContext(nodeBase);
            AddChild(Container);
            
        }
    }
}