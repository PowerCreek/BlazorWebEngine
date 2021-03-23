using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.Classes.Types.Vector;
using BlazorWebEngine.CustomElementContexts;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.ElementManagement.ElementProperties;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorWebEngine.CustomNodes
{
    public class WindowingService : OperationBase
    {
        
        public ContainerContext ContainerContext { get; set; }
        

        public Dictionary<WindowContext, WindowContext> WindowContextMap = new();
        public Dictionary<TabContext, TabContext> TabContextMap = new();
        public List<WindowContext> WindowRenderOrder = new();
        
        public WindowingService(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            AddTabToWindow = PrivateAddTabToWindow;
        }
        
        public Func<WindowContext> CreateWindow { get; set; }

        public WindowContext RegisterWindow(WindowContext context)
        {
            WindowToFront(context);
            WindowContextMap.Add(context, context);
            return context;
        }

        public void WindowToFront(WindowContext context)
        {
            WindowRenderOrder.Remove(context);
            int index = 1;
            foreach (var item in WindowRenderOrder.Where(item => item != context))
            {
                item.WithAttribute("Style", out StyleContext styleContext);
                styleContext.WithStyle(context.StyleOperator, item, ("z-index",$"{index++}"));
            }

            WindowRenderOrder.Add(context);
            context.WithAttribute("Style", out StyleContext styleContext2);
            styleContext2.WithStyle(context.StyleOperator, context, ("z-index",$"{index}"));
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

        #region Window Interaction
        //REGION//REGION//REGION//REGION
        public ElementContext Spacer { get; set; }
        public WindowContext WindowDraggingWithTitlebar { get; set; }
        public WindowContext WindowDraggingWithResize { get; set; }
        public Position ScreenStartPos { get; set; }
        public Position CurrentScreenPos { get; set; }
        public Position BeforeScreenPos { get; set; }
        public Position DeltaPos { get; set; }

        public const int MIN_WIDTH = 80;
        public const int MIN_HEIGHT = 60;

        public void ContainerMouseLeave(dynamic args) => EndTitleDrag();
        
        public void ContainerMouseUp(dynamic args)
        {
            LeftMouseDown = false;
            EndTitleDrag();
            EndResizeDrag();
        }
        
        public void ContainerMouseDown(dynamic args)
        {
            LeftMouseDown = true;
        }
            
        private int PastX = 0;
        private int PastY = 0;
        
        public void ContainerMouseMove(dynamic args)
        {
            
            if (CurrentScreenPos != null)
            {
                BeforeScreenPos = new((int) args.ScreenX, (int) args.ScreenY);
                DeltaPos = new(CurrentScreenPos.X - BeforeScreenPos.X, CurrentScreenPos.Y - BeforeScreenPos.Y);
            }

            CurrentScreenPos = BeforeScreenPos;
            
            if (WindowDraggingWithTitlebar != null)
            {
                WindowDraggingWithTitlebar.Transform.Position.X -= DeltaPos.X;
                WindowDraggingWithTitlebar.Transform.Position.Y -= DeltaPos.Y;
            }

            if (WindowDraggingWithResize != null && (DirX != 0 || DirY != 0) )
            {
                var changeX = 0;
                var changeW = 0;
                var changeH = 0;
                if (DirX == -1)
                {
                    Console.WriteLine(-1);
                    changeX = DeltaPos.X;
                    changeW = changeX;
                    if ((DeltaPos.X+PastX) <= 0 && WindowDraggingWithResize.Transform.Size.Width <= MIN_WIDTH)
                    {
                        PastX += DeltaPos.X;
                        changeW = changeX = 0;
                        changeH = MIN_WIDTH-WindowDraggingWithResize.Transform.Size.Width;
                    }
                }

                if (DirX == 1)
                {
                    changeW = -DeltaPos.X;
                    if((DeltaPos.X+PastX) >= 0 && WindowDraggingWithResize.Transform.Size.Width <= MIN_WIDTH)
                    {
                        PastX += DeltaPos.X;
                        changeW = MIN_WIDTH-WindowDraggingWithResize.Transform.Size.Width;
                    }
                }

                if (DirY == -1)
                {
                    //changeY = DeltaPos.Y;
                    //changeH = changeY;
                }

                if (DirY == 1)
                {
                    changeH = -DeltaPos.Y;
                    if((DeltaPos.Y+PastY) >= 0 && WindowDraggingWithResize.Transform.Size.Height <= MIN_HEIGHT)
                    {
                        PastY += DeltaPos.Y;
                        changeH = MIN_HEIGHT-WindowDraggingWithResize.Transform.Size.Height;
                    }
                }

                if (DirX != 0)
                {
                    WindowDraggingWithResize.Transform.Position.X -= changeX;
                    WindowDraggingWithResize.Transform.Size.Width += changeW;
                }

                if (DirY != 0)
                {
                    WindowDraggingWithResize.Transform.Size.Height += changeH;
                }
            }
        }

        public void EndTitleDrag()
        {
            WindowDraggingWithTitlebar = null;
            Spacer.Get<Transform>("Transform").Size = new(0, 0);
        }
        public void EndResizeDrag()
        {
            if (WindowDraggingWithResize == null) return;
                WindowDraggingWithResize = null;
        }

        public void StartDragAction(dynamic args)
        {
            CurrentScreenPos = ScreenStartPos = new((int)args.ScreenX,(int)args.ScreenY);
        }

        public void WindowTitleBarDown(dynamic args, WindowContext windowContext)
        {
            StartDragAction(args);
            WindowDraggingWithTitlebar = windowContext;
            Spacer.Get<Transform>("Transform").Position =
                new(windowContext.Transform.Position.X, windowContext.Transform.Position.Y);
            Spacer.Get<Transform>("Transform").Size =
                new(windowContext.Transform.Size.Width, windowContext.Transform.Size.Height);
        }
        
        public void WindowResizeDown(dynamic args, WindowContext windowContext)
        {
            StartDragAction(args);
            WindowDraggingWithResize = windowContext;
        }

        public void WindowTitleBarUp(dynamic args, WindowContext windowContext)
        {
            EndTitleDrag();
            
        }
        
        public void WindowResizeUp(dynamic args)
        {
            EndResizeDrag();
        }

        private string LastCursor { get; set; } = "";
        
        private int DirX = 0;
        private int DirY = 0;

        public bool LeftMouseDown = false;
        
        public void WindowMouseMove(dynamic args, WindowContext windowContext)
        {
            if (LeftMouseDown) return;
            if (args.Buttons != 0) return;
            
            DirX = 0;
            DirY = 0;
            PastX = 0;
            PastY = 0;
            
            Position Offset = new((int)args.OffsetX,(int) args.OffsetY);
            Size Bounds = windowContext.Transform.Size;
            
            string cursor = "";

            if (Offset.X <= 0 || Offset.X >= Bounds.Width - 8)
            {
                (cursor, DirX) = Offset.X <= 0 ? ("w", -1) : ("e", 1);
            }

            if (Offset.Y <= 8 || Offset.Y >= Bounds.Height - 8)
            {
                (cursor, DirY) = Offset.Y <= 8 ? ("n" + cursor, -1) : ("s"+ cursor, 1) ;
            }

            if (cursor == "n")
            {
                cursor = "";
            }
            
            if (cursor != "")
            {
                cursor = $"{cursor}-resize";
            }
            
            if (cursor == "" && LastCursor != "")
            {
                ContainerContext.SetCursor(cursor);
            }

            if (cursor != "" && cursor != LastCursor)
            {
                ContainerContext.SetCursor(cursor);
            }
            LastCursor = cursor;
            
        }

        public void WindowMouseLeave(dynamic args)
        {
            if (LastCursor != "")
            {
                LastCursor = "";
            }
            ContainerContext.SetCursor("default");
        }

        //REGION//REGION//REGION//REGION
        #endregion
    }
}