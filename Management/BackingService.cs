using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components;

namespace BlazorWebEngine.Management
{
    public class BackingService
    {
        public ElementInformation ElementInformation { get; init; }
        public ElementManager ElementManager { get; init; }
        public OperationManager OperationManager { get; init; }
        
        public BackingService(IBuilder backBuilder, ElementRegistry elementRegistry, ElementInformation elementInformation, 
            ElementManager elementManager, OperationManager operationManager)
        {
            OperationManager = operationManager;
            ElementInformation = elementInformation;
            ElementManager = elementManager;
            
            backBuilder.Build(this);
        }
    }
}