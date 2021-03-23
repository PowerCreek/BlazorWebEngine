using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Classes.Types.Vector;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.ElementManagement.ElementProperties;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class WindowContext : ElementContext
    {
        public WindowingService WindowingService { get; init; }
        
        public readonly Transform Transform;
        public StyleOperator StyleOperator { get; init; }
        public TabGroupContext TabGroupContext { get; init; }
        public WindowContent WindowContent { get; init; }
        
        public WindowContext(NodeBase nodeBase) : base($"Window{nodeBase.Id}", nodeBase)
        {
            WindowingService = nodeBase.ElementServices.OperationManager.GetOperation<WindowingService>();
            
            AddChild(TabGroupContext = new TabGroupContext(NodeBase));

            TabGroupContext.OnTabSelected = OnTabSelected;
            
            AddChild(WindowContent = new WindowContent(NodeBase));
            
            OnBuild += (context, builder) =>
            {
                builder.Open("div");
                builder.WithAttribute("style", "position: relative; place-self: stretch; background-color: black");
                builder.End();
            };
            
            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator=nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "absolute"),
                ("background-color", "red"),
                ("display","grid"),
                ("grid-template-rows","20px min-content auto")
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