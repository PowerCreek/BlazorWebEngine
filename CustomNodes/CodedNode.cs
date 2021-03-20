using System;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Classes.Contexts;
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
        public StyleOperator StyleOperator;
        public RenderBuilder RenderBuilder = new RenderBuilder();
        
        public ElementContext Header {get;set;}
        public ElementContext Center {get;set;}
        
        public CodedNode(
            IElementServices elementServices) : 
            base(elementServices)
        {
            StyleOperator = ElementServices.OperationManager.GetOperation<StyleOperator>();
        }

        public override void Instantiate()
        {
            StyleContext headerStyle = new StyleContext();

            Header = ElementServices.ElementContextProvider.AddContext(new ElementContext(
                $"header_{Id}",
                (builder) =>
                {
                    builder.Add<Transform>("Transform", out Transform transform)
                        .Add("StyleContext", headerStyle);

                    headerStyle.AddStyle(StyleOperator,
                        builder.Id,
                        ("background-color","red"),
                        ("position","absolute"),
                        ("width","100px"),
                        ("height","100px"));
                    
                    transform.OnMove = (a, b) => headerStyle.AddStyle(StyleOperator, builder.Id, 
                        ("left",$"{b.X}px"),
                        ("top",$"{b.Y}px"));
                    
                    transform.OnResize += (a, b) => headerStyle.AddStyle(StyleOperator, builder.Id, 
                        ("left",$"{b.Width}px"),
                        ("top",$"{b.Height}px"));

                    transform.Position = new Position(100,300); }));
            
            Header.RenderElement = builder => RenderBuilder.Open<TestComp>(builder)
                .WithAttribute("id",Header.Id)
                //.WithStyles(Header.Get<StyleContext>("StyleContext"))
                .End<TestComp>();
            
            StyleContext centerStyle = new StyleContext();

            Center = ElementServices.ElementContextProvider.AddContext(new ElementContext(
                $"center_{Id}",
                (builder) =>
                {
                    builder.Add<Transform>("Transform", out Transform transform)
                        .Add("StyleContext", centerStyle);

                    centerStyle.AddStyle(StyleOperator,
                        builder.Id,
                        ("background-color","red"),
                        ("position","absolute"),
                        ("width","100px"),
                        ("height","100px"));
                    
                    transform.OnMove = (a, b) => centerStyle.AddStyle(StyleOperator, builder.Id, 
                        ("left",$"{b.X}px"),
                        ("top",$"{b.Y}px"));
                    
                    transform.OnResize = (a, b) =>
                    {
                        centerStyle.AddStyle(StyleOperator, builder.Id,
                            ("left", $"{b.Width}px"),
                            ("top", $"{b.Height}px"));
                    };

                    transform.Position = new Position(300,300);
                    Task.Run(async () =>
                    {
                        await Task.Delay(3000);
                        transform.Position = new Position(500, 500);
                        
                        ElementServices.ComponentMap.GetComponent<Basic>(Center.Id)?.TriggerStateChanged();
                    });

                }));            
            
            Center.RenderElement = builder => RenderBuilder.Open<TestComp>(builder)
                .WithAttribute("id",Center.Id)
                .End<TestComp>();
        }

        public void RenderOutline(RenderTreeBuilder builder)
        {
            Header.RenderElement(builder);
            Center.RenderElement(builder);
        }
    }
}