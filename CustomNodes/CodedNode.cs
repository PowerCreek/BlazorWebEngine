using BlazorWebEngine.CustomElementContexts;
using BlazorWebEngine.Interfaces;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.CustomNodes
{
    public class CodedNode : NodeBase, IRenderable
    {
        //can be made into a dependency, but will stay here for now

        public CodedNode(
            IElementServices elementServices) :
            base(elementServices)
        {
        }

        public ElementContext Header { get; set; }
        public ElementContext Center { get; set; }

        public void RenderOutline(RenderTreeBuilder builder)
        {
            ElementServices.RenderBuilder.index = 0;
            RootContext?.BuildRenderFragment(ElementServices.RenderBuilder, builder);
        }

        public override void Instantiate()
        {
            var BodyContext = new BodyContextExample(this, out RootContext);
        }
    }
}