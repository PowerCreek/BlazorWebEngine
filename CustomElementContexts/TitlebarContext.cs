using System;
using System.Net.Http.Json;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.CustomElementContexts
{
    public class TitlebarContext : ElementContext
    {
        public StyleOperator StyleOperator { get; init; }

        public TitlebarContext(NodeBase nodeBase) : base($"WindowTitle{nodeBase.Id}", nodeBase)
        {
                    
            WithAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator = nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "relative"),
                ("background-color", "black"),
                ("place-self","stretch")
            );

            ElementEventHandler.AddEvent(nameof(OnMouseDown).ToLower(), OnMouseDown);
            ElementEventHandler.AddEvent(nameof(OnMouseUp).ToLower(), OnMouseUp);

            OnBeforeBuild = (context, builder) =>
            {
                builder.RenderTreeBuilder.AddEventPreventDefaultAttribute(builder.index++,
                    "onmousemove", true);
            };

        }

        public Action<dynamic> TitlebarMouseDown { get; set; } 
        public Action<dynamic> TitlebarMouseUp { get; set; } 

        public void OnMouseDown(dynamic args)
        {
            TitlebarMouseDown?.Invoke(args);
        }
        public void OnMouseUp(dynamic args)
        {
            TitlebarMouseUp?.Invoke(args);
        }
        

        public void RenderTitlebar(ElementContext elementContext, RenderBuilder builder)
        {
            
        }
    }
}