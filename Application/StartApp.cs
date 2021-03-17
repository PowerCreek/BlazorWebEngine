

using System;
using System.Linq;
using BlazorWebEngine.Classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Application
{

    public class StartApp
    {
        private IJSRuntime JsRuntime { get; init; }

        private Container Container { get; init; }
        
        public StartApp(IJSRuntime runtime)
        {
            JsRuntime = runtime;
            Container = new Container(JsRuntime);
            InitComponents();
        }
        
        public void InitComponents()
        {
            var hold1 = Container.RegisterElement("#test1",
                new ElementDefinition("test1")
                    .OpenElement("div")
                    .WithId("test1")
                    .WithAttributes(("tabindex","0"))
                    .OpenElement("div")
                    .CloseElement()
                    .CloseElement(),
                out RegistrationData registrationData1
            );

            registrationData1.StyleRef.StyleCallback(hold1, "background", ("background-color","red"));
            
            hold1.Transform.Position = new(400,400);
            hold1.Transform.Size = new(200,200);
        }

        public RenderFragment RenderComponents()
        {
            return b =>
            {
                b.OpenElement(0, "div");
                foreach (var r in Container.ComponentStorage.ComponentMap.Values)
                {
                    b.AddContent(1,r.ComponentDef.GetFragment());
                }
                b.CloseElement();
                
                b.OpenElement(1, "div");
                foreach (var v in Container.StyleRefInvoker.SelectorMap.Values)
                {
                    b.AddContent(2,v.GetStyleElement());
                }
                b.CloseElement();

            };
        }
    }
}