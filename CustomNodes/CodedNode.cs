using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Classes.Types.Vector;
using BlazorWebEngine.Components;
using BlazorWebEngine.Interfaces;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.ElementManagement.ElementProperties;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.CustomNodes
{
    public class CodedNode : NodeBase, IRenderable
    {
        public RenderBuilder RenderBuilder = new();
        public readonly StyleOperator StyleOperator;

        public CodedNode(
            IElementServices elementServices) :
            base(elementServices)
        {
            StyleOperator = ElementServices.OperationManager.GetOperation<StyleOperator>();
        }

        public ElementContext Header { get; set; }
        public ElementContext Center { get; set; }

        public void RenderOutline(RenderTreeBuilder builder)
        {
            RenderBuilder.index = 0;
            Header.BuildRenderFragment(RenderBuilder, builder);
        }

        public override void Instantiate()
        {
            Header = ElementServices.ElementContextProvider.AddContext(new ElementContext($"header_{Id}", b=>
            {
                b.Add("Transform", out Transform transform)
                    .Add("Style", out StyleContext styleContext);

                styleContext.AddStyle(StyleOperator, b, 
                    ("position","absolute"),
                    ("background-color","blue"),
                    ("left","100px"),
                    ("top","400px"),
                    ("width","100px"),
                    ("height","100px")
                    );

                b.OnBuild += (ctx, bld) =>
                {
                    bld.WithAttribute("style", styleContext.GetRenderOutput());
                };
                
            })); 
            
            Center = ElementServices.ElementContextProvider.AddContext(new ElementContext($"center{Id}", b=>
            {
                b.Add("Transform", out Transform transform)
                    .Add("Style", out StyleContext styleContext);

                styleContext.AddStyle(StyleOperator, b, 
                    ("position","absolute"),
                    ("background-color","red"),
                    ("left","100px"),
                    ("top","100px"),
                    ("width","100px"),
                    ("height","100px")
                    );

                b.OnBuild += (ctx, bld) =>
                {
                    bld.WithAttribute("style", styleContext.GetRenderOutput());
                };
            }));

            Header.AddChild(Center);

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                
                Header.Get<StyleContext>("Style").AddStyle(StyleOperator, Header,
                    ("background-color","black"));
                
                await Task.Delay(3000);
                Header.StateComponentReference.Reset();
            });

        }
    }
}