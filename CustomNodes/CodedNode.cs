using System;
using System.Dynamic;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
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
            int id, 
            OperationManager operationManager, 
            NodeInformation nodeInformation) : 
            base(id, operationManager, nodeInformation)
        {
            StyleOperator = OperationManager.GetOperation<StyleOperator>();
        }

        public override void Instantiate()
        {
            StyleContext headerStyle = new StyleContext();

            Header = new ElementContext(
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
                    
                    transform.OnResize = (a, b) => headerStyle.AddStyle(StyleOperator, builder.Id, 
                        ("left",$"{b.Width}px"),
                        ("top",$"{b.Height}px"));

                    transform.Position = new Position(100,300); });
            
            Header.RenderElement = builder => RenderBuilder.Open(builder, "div")
                .WithAttribute("id",Header.Id)
                .WithStyles(Header.Get<StyleContext>("StyleContext"))
                .End();
            
            StyleContext centerStyle = new StyleContext();

            Center = new ElementContext(
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
                    
                    transform.OnResize = (a, b) => centerStyle.AddStyle(StyleOperator, builder.Id, 
                        ("left",$"{b.Width}px"),
                        ("top",$"{b.Height}px"));

                    transform.Position = new Position(300,300);
                });            

            
            Center.RenderElement = builder => RenderBuilder.Open(builder, "div")
                .WithAttribute("id",Center.Id)
                .WithStyles(Center.Get<StyleContext>("StyleContext"))
                .End();
        }

        public void RenderOutline(RenderTreeBuilder builder)
        {           

            Header.RenderElement(builder);
            Center.RenderElement(builder);
        }
    }
}