using System.Threading.Tasks;
using BlazorWebEngine.Classes.Contexts;
using BlazorWebEngine.CustomNodes;
using BlazorWebEngine.Management.ElementManagement;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.CustomElementContexts
{
    public class ContainerContext : ElementContext
    {
        public ContainerContext(NodeBase nodeBase) : base($"Container{nodeBase.Id}", nodeBase)
        {
            WindowingService = nodeBase.ElementServices.OperationManager.GetOperation<WindowingService>();
            
            WindowingService.CreateWindow = CreateWindow;

            AddAttribute("Style", out StyleContext styleContext);
            styleContext.WithStyle(nodeBase.ElementServices.OperationManager.GetOperation<StyleOperator>(),
                this,
                ("position", "absolute"),
                ("width", "100%"),
                ("height", "100%"),
                ("background-color", "gray")
            );
            
            //use tabdata provider to create windows in the future.

            var noteTab = WindowingService.CreateTabData<NoteTab>(nodeBase);
            WindowContext windowContext = CreateWindow();
            WindowingService.AddTabToWindow(windowContext, noteTab);

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                var noteTab2 = WindowingService.CreateTabData<NoteTab2>(nodeBase);
                WindowingService.AddTabToWindow(windowContext, noteTab2);
            });


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