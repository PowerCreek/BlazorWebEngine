using System;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class TabData
    {
        public TabContext TabContext { get; set; }        
        public WindowContext WindowSource { get; set; }

        public TabData(NodeBase nodeBase)
        {
        }
    }
    
    public class TabData<T> : TabData where T: TabContext
    {

        public TabData(NodeBase nodeBase) : base(nodeBase)
        {
            TabContext = (T) Activator.CreateInstance(typeof(T),nodeBase);
        }
    }

    public class NoteTab : TabContext
    {
        
        public NoteTab(NodeBase nodeBase) : base(nodeBase)
        {
            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator,
                this,
                ("position", "relative"),
                ("width","100%"),
                ("height","100%"),
                ("place-self","stretch"),
                ("background-color", "green")
            );
        }
    }
    
    public class NoteTab2 : TabContext
    {
        
        public NoteTab2(NodeBase nodeBase) : base(nodeBase)
        {
            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(StyleOperator,
                this,
                ("position", "relative"),
                ("width","100%"),
                ("height","100%"),
                ("place-self","stretch"),
                ("background-color", "red")
            );
        }
    }
}