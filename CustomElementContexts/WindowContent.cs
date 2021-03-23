using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Components;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class WindowContent : ElementContext
    {
        public ElementAssociationOperator AssignOperator { get; init; }
        public StyleOperator StyleOperator { get; init; }
        public WindowContent(NodeBase nodeBase) : base($"WindowContent{nodeBase.Id}", nodeBase)
        {
            AssignOperator = nodeBase.ElementServices.OperationManager.GetOperation<ElementAssociationOperator>();
            
            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator = nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "relative"),
                ("background-color", "white"),
                ("place-self","stretch")
            );
        }

        public void SetChild(TabContext tabContext)
        {
            Children.Clear();
            AddChild(tabContext);
            tabContext.Display(true);
            StateComponentReference?.Reset();
        }
    }
}