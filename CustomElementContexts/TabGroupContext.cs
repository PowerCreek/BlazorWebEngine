using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebEngine.CustomElementContexts
{
    public class TabGroupContext : ElementContext
    {
        public StyleOperator StyleOperator { get; init; }
        
        private readonly Dictionary<TabData, TabData> TabMap = new ();
        public List<TabData> TabSelectOrder = new ();
        public List<TabData> TabOrder = new();
        
        public TabData SelectedTab
        {
            get => TabSelectOrder.LastOrDefault();
            set { TabSelectOrder.Remove(value); TabSelectOrder.Add(value);
                StateComponentReference?.Reset();
            }
        }

        public TabGroupContext(NodeBase nodeBase) : base($"TabGroup{nodeBase.Id}", nodeBase)
        {
            
            WithAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator=nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "relative"),
                ("background-color", "orange"),
                ("place-self","stretch"),
                ("display","grid"),
                ("grid-auto-columns","min-content"),
                ("overflow","hidden"),
                ("grid-auto-flow","column"),
                ("user-select","none")
            );
            //position: relative;background-color: green;width: 100%;display: grid;grid-auto-columns: min-content;
            //overflow: hidden;grid-auto-flow: column;min-height: 30px
            OnBeforeBuild = (context, builder) =>
            {
                builder.WithAttribute("draggable", "false");
            };
            
            OnAfterBuild = RenderTabs;
        }

        public Action<TabData> OnTabSelected { get; set; }

        public void RenderTabs(ElementContext elementContext, RenderBuilder builder)
        {
            TabData selected = SelectedTab;
            
            foreach (TabData tab in TabOrder)
            {
                builder.Open(builder.RenderTreeBuilder, "div");
                builder.WithAttribute("class", "tab");
                builder.WithAttribute("draggable", "true");
                builder.WithAttribute("style",
                    $"place-self: center; background-color: cyan; margin: 2px 4px; border: 2px solid {(tab==SelectedTab?"white":"gray")};");
                
                //builder.WithEventListener("onclick", () => { OnTabClick(tab); });
                builder.WithEventListener("onmousedown", () => { OnTabDown(tab); });
                builder.WithEventListener("onmouseup", () => { OnTabUp(tab); });
                builder.WithEventListener("onmouseleave", () => { OnTabLeave(tab); });
                
                builder.RenderTreeBuilder.AddEventStopPropagationAttribute(builder.index++, "onmousemove", true);
                
                
                builder.WithContent(tab.TabContext.Id[^6..]);
                builder.End();
            }
        }
        
        private bool CancelClick = false;
        private TabData TabDown { get; set; }
        
        private void OnTabClick(TabData tabData)
        {
            if (!CancelClick) return;
            SelectedTab = tabData;
            OnTabSelected?.Invoke(tabData);
        }

        private void OnTabLeave(TabData tabData)
        {
            TabDown = null;
        }
        
        private void OnTabDown(TabData tabData)
        {
            CancelClick = false;
            TabDown = tabData;
        }

        private void OnTabUp(TabData tabData)
        {
            CancelClick = true;
            if (TabDown == tabData)
            {
                OnTabClick(tabData);
            }
            TabDown = null;
        }

        public void AddTab(TabData add)
        {
            TabOrder.Remove(add); 
            TabOrder.Add(add);
            TabMap.Add(add,add);
            SelectedTab = add;
            StateComponentReference?.Reset();
        }

        public void RemoveTab(TabData tabData)
        {
            TabOrder.Remove(tabData);
            TabSelectOrder.Remove(tabData);
        }
    }
}