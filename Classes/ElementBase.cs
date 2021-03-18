using System;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;

namespace BlazorWebEngine.Classes
{
    public class ElementBase : IInstance
    {

        public int id { get; set; }
        public int Parent { get; set; }
        
        public ElementBase()
        {
            
        }
        
        public virtual void Instantiate(ElementManager elementManager)
        {
            Console.WriteLine("ElementBase");
            
        }
        
    }
}