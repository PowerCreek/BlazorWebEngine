using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Classes.Types.Vector;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Interfaces;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.ElementManagement.ElementProperties;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.CustomElementContexts
{
   public class WindowContext : ElementContext
    {
        public WindowingService WindowingService { get; init; }
        
        public readonly Transform Transform;
        public StyleOperator StyleOperator { get; init; }
        
        public TitlebarContext TitlebarContext { get; init; }
        public TabGroupContext TabGroupContext { get; init; }
        public WindowContent WindowContent { get; init; }
        
        public WindowContext(NodeBase nodeBase) : base($"Window{nodeBase.Id}", nodeBase)
        {
            WindowingService = nodeBase.ElementServices.OperationManager.GetOperation<WindowingService>();

            CssClass = "window-context";
            
            AddChild(TitlebarContext = new TitlebarContext(NodeBase));
            TitlebarContext.TitlebarMouseDown = args=>
            {
                Console.WriteLine("here");
                WindowingService.WindowTitleBarDown(args, this);
            };
            TitlebarContext.TitlebarMouseUp = args=>
            {
                WindowingService.WindowTitleBarUp(args, this);
            };
            
            AddChild(TabGroupContext = new TabGroupContext(NodeBase));

            TabGroupContext.OnTabSelected = OnTabSelected;
            
            AddChild(WindowContent = new WindowContent(NodeBase));
            
            WithAttribute("Style", out StyleContext styleContext);
            
            styleContext.WithStyle(StyleOperator=nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "absolute")
            );
            
            Add("Transform", out Transform);
            Transform.OnResize = (a,b) =>
            {
                styleContext.WithStyle(StyleOperator, this, 
                    ("width",$"{b.Width}px"),
                    ("height",$"{b.Height}px")
                );
            };
            
            Transform.OnMove = (a,b) =>
            {
                styleContext.WithStyle(StyleOperator, this, 
                    ("left",$"{b.X}px"),
                    ("top",$"{b.Y}px")
                );
            };
            
            Transform.Position = new Position(200, 200);
            Transform.Size = new Size(200, 300);

            OnBeforeBuild += (a, b) =>
            {
                b.WithAttribute("tabindex", 0);
                b.WithAttribute("draggable", false);
                b.RenderTreeBuilder.AddEventPreventDefaultAttribute(b.index++, "ondragstart", true);
                b.RenderTreeBuilder.AddEventPreventDefaultAttribute(b.index++, "ondragover", true);
                
                b.RenderTreeBuilder.AddEventStopPropagationAttribute(b.index++, "ondragstart", true);
                b.RenderTreeBuilder.AddEventStopPropagationAttribute(b.index++, "ondragover", true);
                
                b.RenderTreeBuilder.AddEventStopPropagationAttribute(b.index++, "ondrop", true);
                b.RenderTreeBuilder.AddEventStopPropagationAttribute(b.index++, "ondrop", true);
            };
            
            ElementEventHandler.AddEvent("onmousedown", OnMouseDown);
            ElementEventHandler.AddEvent("onmouseup", WindowingService.WindowResizeUp);
            ElementEventHandler.AddEvent("onmousemove", OnMouseMove);
            ElementEventHandler.AddEvent("onmouseleave", OnMouseLeave);
        }
        
        public void OnMouseDown(dynamic args)
        {
            WindowingService.WindowResizeDown(args, this);
        }
        
        public void OnMouseMove(dynamic args)
        {
            WindowingService.WindowMouseMove(args, this);
        }
        
        public void OnMouseLeave(dynamic args)
        {
            WindowingService.WindowMouseLeave(args);
        }

        public void OnTabSelected(TabData tabData)
        {
            WindowContent.SetChild(tabData.TabContext);
        }

        public void AddTab(TabData add)
        {
            add.WindowSource = this;
            OnTabSelected(add);
            TabGroupContext.AddTab(add);
        }
    }
}