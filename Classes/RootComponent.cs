using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Classes
{
    public class RootComponent
    {
        public RootComponent(ElementManager elementManager, OperationManager operationManager)
        {
            Init(elementManager, operationManager);
        }

        public void Init(ElementManager elementManager, OperationManager operationManager)
        {
            elementManager.GenerateItem<TestElement>(operationManager);
            
        }
        
        public RenderFragment RenderElements()
        {
            return b =>
            {
                
            };
        }

    }
}