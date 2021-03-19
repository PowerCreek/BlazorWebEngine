using System;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Management.ElementHandling
{
    public class ElementManager
    {
        public ElementRegistry ElementRegistry { get; set; }
        public ElementInformation ElementInformation { get; set; }
        
        public ElementManager(ElementRegistry elementRegistry, ElementInformation elementInformation)
        {
            ElementRegistry = elementRegistry;
            ElementInformation = elementInformation;
        }

        public void GenerateItem<T>(OperationManager operationManager) where T : IInstance
        {
            IInstance item = (IInstance) Activator.CreateInstance(typeof(T), ElementRegistry.AddElement(), operationManager, ElementInformation);
            GenerateItem<T>(item);
        }

        private void GenerateItem<T>(object t) where T: IInstance
        {
            if (!(t is ElementBase hold)) return;
            hold.Instantiate();
        }
    }
    
}