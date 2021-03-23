using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class TabContext : ElementContext{
        public WindowingService WindowingService { get; init; }
        public StyleOperator StyleOperator { get; init; }
        
        public TabContext(NodeBase nodeBase) : base($"TabContext{nodeBase.Id}", nodeBase)
        {
            WindowingService = nodeBase.ElementServices.OperationManager.GetOperation<WindowingService>();
            StyleOperator = nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>();
        }

        public virtual void Display(bool show)
        {
            if (show)
            {
                WithAttribute("Style", out StyleContext item);
                    item.WithoutStyles(StyleOperator, this, "display");
            }
            else
            {
                WithAttribute("Style", out StyleContext item);
                    item.WithStyle(StyleOperator, this, ("display","none"));
            }
        }
    }
}