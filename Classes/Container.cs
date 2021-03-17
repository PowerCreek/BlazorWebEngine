using System;
using System.Numerics;
using BlazorWebEngine.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Classes
{
    public class Container : Generator
    {
        public readonly int ContainerId = NextIndex;

        public IJSRuntime JsRuntime { get; init; }
        
        public Container(IJSRuntime runtime)
        {
            JsRuntime = runtime;

        }

        /// <summary>
        ///  Register Element sets up the elements with Transform style callbacks
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ComponentData RegisterElement(string selector, ElementDefinition def, out RegistrationData reg)
        {
            RegistrationData data = RegisterComponent(out int index, selector);
            data.ComponentData.ComponentDef = def;

            var styleCallback = data.StyleRef.PerformStyleAction;
            styleCallback(index, selector, "positioning", ("position", "absolute"));

            data.ComponentData.Transform.OnPosition = vec => styleCallback(index, selector, "location",
                ("left", $"{vec.X}px"),
                ("top", $"{vec.Y}px")
            );

            data.ComponentData.Transform.OnSize = vec => styleCallback(index, selector, "size",
                ("width", $"{vec.X}px"),
                ("height", $"{vec.Y}px")
            );
            
            data.ComponentData.DoSetParent = 
            parent =>
            {
                if (data.ComponentData.Parent != null)
                {

                }

                data.ComponentData.Parent = parent;
                //invoke parent append child

                string fixParent = data.ComponentData.Parent;
                if (fixParent[0].Equals('#'))
                {
                    fixParent = fixParent[1..];
                }
                string fixChild = data.ComponentData.Selector;
                if (fixChild[0].Equals('#'))
                {
                    fixChild = fixChild[1..];
                }
                
                JsRuntime.InvokeVoidAsync(
                    "SetParent", 
                    fixParent,
                    fixChild);
            };
            reg = data;
            return data.ComponentData;
        }
    }
    

    public class Generator
    {
        
        public readonly StyleRef StyleRefInvoker;
        public readonly ComponentStorage ComponentStorage;

        // ReSharper disable once CA2211
        public static int Indexor;
        public static int NextIndex => Indexor++;

        public Action TriggerStateChange { get; set; } = () => { };

        public Generator()
        {
            StyleRefInvoker = new StyleRef(SetStyles);
            ComponentStorage = new ComponentStorage();
        }

        /*
        * Returns data for a newly registered component to use.
        */
        public RegistrationData RegisterComponent(out int index, string selector)
        {
            index = NextIndex;
            
            var retData = new RegistrationData(
                index,
                selector,
                StyleRefInvoker,
                ComponentStorage.AddComponent(index, new ComponentData(index))
            );
            return retData;
        }

        public virtual void SetStyles(int id, string selector, string styleKey, params (string, string)[] styles)
        {
            ComponentData component = ComponentStorage.GetComponentByCondition(a => a.ComponentId == id);
            StyleRefInvoker.ApplyStyles(selector, styleKey, styles);
        }
    }
}