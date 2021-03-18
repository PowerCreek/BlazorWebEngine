using System;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Classes
{
    public class TestElement : ElementBase
    {

        public TestElement(OperationManager operationManager)
        {
            operationManager.GetOperation<StyleOperator>();
        }
        
        public override void Instantiate(ElementManager elementManager)
        {
            Console.WriteLine("do thing TestElement");
        }
    }
}