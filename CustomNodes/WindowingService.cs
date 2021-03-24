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
        
        public void ContainerMouseMove(dynamic args)
        {
            if (CurrentScreenPos != null)
            {
                //BeforeScreenPos = new((int) args.ScreenX, (int) args.ScreenY);
                BeforeScreenPos = new((int) args.ClientX, (int) args.ClientY);
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
                Position RPos = new Position(WindowDraggingWithResize.Transform.Position.X - CurrentScreenPos.X,
                    WindowDraggingWithResize.Transform.Position.Y - CurrentScreenPos.Y);
                
                var changeX = WindowDraggingWithResize.Transform.Position.X;
                var changeY = WindowDraggingWithResize.Transform.Position.Y;
                var changeW = WindowDraggingWithResize.Transform.Size.Width;
                var changeH = WindowDraggingWithResize.Transform.Size.Height;
                
                if (DirX == -1)
                {
                    int nX = CurrentScreenPos.X;
                    int nW = WindowDraggingWithResize.Transform.Size.Width-(nX-WindowDraggingWithResize.Transform.Position.X);
                    if (nW <= MIN_WIDTH)
                    {
                        changeX += changeW-MIN_WIDTH;
                        changeW = MIN_WIDTH;
                    }
                    else
                    {
                        changeX = CurrentScreenPos.X;
                        changeW = nW;
                    }
                }
                if (DirX == 1)
                {
                    int nX = CurrentScreenPos.X;
                    int nW = CurrentScreenPos.X-changeX;
                    changeW = nW <= MIN_WIDTH ? MIN_WIDTH : nW;
                }

                if (DirY == -1)
                {
                    int nY = CurrentScreenPos.Y;
                    int nH = WindowDraggingWithResize.Transform.Size.Height-(nY-WindowDraggingWithResize.Transform.Position.Y);
                    if (nH <= MIN_HEIGHT)
                    {
                        changeY += changeH-MIN_HEIGHT;
                        changeH = MIN_HEIGHT;
                    }
                    else
                    {
                        changeY = CurrentScreenPos.Y;
                        changeH = nH;
                    }
                }

                if (DirY == 1)
                {
                    int nY = CurrentScreenPos.Y;
                    int nH = CurrentScreenPos.Y-changeY;
                    changeH = nH <= MIN_HEIGHT ? MIN_HEIGHT : nH;
                }

                if (DirX != 0)
                {
                    WindowDraggingWithResize.Transform.Position.X = changeX;
                    WindowDraggingWithResize.Transform.Size.Width = changeW;
                }

                if (DirY != 0)
                {
                    WindowDraggingWithResize.Transform.Position.Y = changeY;
                    WindowDraggingWithResize.Transform.Size.Height = changeH;
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
                
            ContainerContext.SetCursor("default");
        }

        public void StartDragAction(dynamic args)
        {
            CurrentScreenPos = ScreenStartPos = new((int)args.ClientX,(int)args.ClientY);
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
            
            //Position Offset = new((int)args.ScreenX - windowContext.Transform.Position.X, 
            //    (int) args.ScreenY - windowContext.Transform.Position.Y);
            Position Offset = new((int)args.ClientX - windowContext.Transform.Position.X, 
                (int) args.ClientY - windowContext.Transform.Position.Y);

            Size Bounds = windowContext.Transform.Size;
            
            string cursor = "";

            int space = 4;

            if (Offset.X <= space || Offset.X >= Bounds.Width - space)
            {
                (cursor, DirX) = Offset.X <= space ? ("w", -1) : ("e", 1);
            }

            if (Offset.Y <= space || Offset.Y >= Bounds.Height - space)
            {
                if (DirX != 0 && Offset.Y <= space)
                {
                    (cursor, DirY) = ("n" + cursor, -1);
                }

                if (Offset.Y >= Bounds.Height - space)
                {
                    (cursor, DirY) = ("s" + cursor, 1);
                }
            }

            if (cursor == "n")
            {
                //cursor = $"{cursor}-resize";
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
            if (LastCursor != "" && !LeftMouseDown)
            {
                LastCursor = "";
                ContainerContext.SetCursor("default");
            }
        }

        public void TabDragStart(dynamic args, TabData tabData)
        {
            Console.WriteLine("starting tab drag");
        }
        
        public void TabDragEnd(dynamic args, TabData tabData)
        {
            Console.WriteLine("Ending tab drag");
        }
        
        //REGION//REGION//REGION//REGION
        #endregion

        public bool IsMouseOverTabGroup = false; 
        public void TabGroupMouseMove(object args)
        {
            IsMouseOverTabGroup = true;
        }
    }
}