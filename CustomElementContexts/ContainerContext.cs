using System;
using System.Threading.Tasks;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.ElementManagement.ElementProperties;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.CustomElementContexts
{
    
    public class ContainerContext : ElementContext
    {
        public StyleOperator StyleOperator { get; init; }
        
        public ContainerContext(NodeBase nodeBase) : base($"Container{nodeBase.Id}", nodeBase)
        {
            WindowingService = nodeBase.ElementServices.OperationManager.GetOperation<WindowingService>();
            WindowingService.ContainerContext = this;
            
            WindowingService.CreateWindow = CreateWindow;

            CssClass = "window-container";

            WithAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator = nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "relative")
            );

            var noteTab = WindowingService.CreateTabData<NoteTab>(nodeBase);
            WindowContext windowContext = CreateWindow();
            WindowingService.AddTabToWindow(windowContext, noteTab);

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                var noteTab2 = WindowingService.CreateTabData<NoteTab2>(nodeBase);
                WindowingService.AddTabToWindow(windowContext, noteTab2);
                WindowingService.WindowToFront(windowContext);
            });
            
            ElementEventHandler.AddEvent("onmousedown", WindowingService.ContainerMouseDown);
            ElementEventHandler.AddEvent("onmousemove", WindowingService.ContainerMouseMove);
            ElementEventHandler.AddEvent("onmouseup", WindowingService.ContainerMouseUp);
            ElementEventHandler.AddEvent("ondragend", WindowingService.ContainerMouseUp);

            ElementContext spacer = new ElementContext($"WindowSpacer{nodeBase.Id}",nodeBase);
            spacer.WithAttribute("Style", out StyleContext spacerStyle);
            var transform = spacer.Get<Transform>("Transform");
            spacerStyle.WithStyle(StyleOperator, spacer, 
                ("position","absolute"),
                ("left","0px"),
                ("top","0px"),
                ("width","0px"),
                ("height","0px"),
                //("visibility","hidden"),
                ("z-index","-1"),
                ("background-color","cyan")
            );

            transform.OnMove = (transform1, pos) =>
            {
                spacerStyle.WithStyle(StyleOperator, spacer, 
                    ("left",$"{pos.X}px"),
                    ("top",$"{pos.Y}px")
                );
            };
            transform.OnResize = (transform1, size) =>
            {
                spacerStyle.WithStyle(StyleOperator, spacer, 
                    ("width",$"{size.Width}px"),
                    ("height",$"{size.Height}px")
                );
            };
            
            AddChild(spacer);
            WindowingService.Spacer = spacer;

            OnBeforeBuild = (context, builder) =>
            {
                builder.RenderTreeBuilder.AddEventStopPropagationAttribute(builder.index++,
                    "onmousemove", true);
            };

        }

        public void SetCursor(string cursor)
        {
            WithAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator,
                this,
                ("cursor", cursor)
            );
        }
        
        public WindowingService WindowingService { get; }
        
        public WindowContext CreateWindow()
        {
            WindowContext windowContext = new WindowContext(NodeBase);
            AddChild(WindowingService.RegisterWindow(windowContext));
            return windowContext;
        }
    }
}