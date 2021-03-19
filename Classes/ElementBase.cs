using System;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Classes
{
    public class ElementBase : IInstance
    {

        public int Id { get; set; }
        public int Parent { get; set; }
        
        public OperationManager OperationManager;
        public ElementInformation ElementInformation;
        
        public ElementBase(int id, OperationManager operationManager, ElementInformation elementInformation)
        {
            Id = id;
            OperationManager = operationManager;
            ElementInformation = elementInformation;
        }
        
        public virtual void Instantiate()
        {
            Console.WriteLine("ElementBase");
            
        }
        
    }
}