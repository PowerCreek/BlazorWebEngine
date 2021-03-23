using System;
using System.Collections.Generic;
using BlazorWebEngine.CustomElementContexts;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.JSInterop;

namespace BlazorWebEngine.CustomNodes
{
    public class WindowingService : OperationBase
    {
        public WindowingService(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            AddTabToWindow = PrivateAddTabToWindow;
        }

        public Dictionary<WindowContext, WindowContext> WindowContextMap = new();
        public Dictionary<TabContext, TabContext> TabContextMap = new();
        
        public Func<WindowContext> CreateWindow { get; set; }

        public WindowContext RegisterWindow(WindowContext context)
        {
            WindowContextMap.Add(context, context);
            return context;
        }

        public TabData<T> CreateTabData<T>(NodeBase nodeBase) where T : TabContext
        {
            return (TabData<T>) Activator.CreateInstance(typeof(TabData<T>), nodeBase);
        }

        public Action<WindowContext, TabData> AddTabToWindow { get; set; }

        private void PrivateAddTabToWindow(WindowContext windowContext, TabData tabData)
        {
            windowContext.AddTab(tabData);
        }

        public void RemoveTabFromWindow()
        {
            
        }
        
    }
}