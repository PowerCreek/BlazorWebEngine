using System;
using System.Numerics;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;
using Newtonsoft.Json.Linq;

namespace BlazorWebEngine.Classes
{
    
    public class TestElement : ElementBase
    {
        
        public Transform Transform => ElementInformation.GetElementData<Transform>(this);
        
        public TestElement(int id, OperationManager operationManager,  ElementInformation elementInformation) : 
            base(id, operationManager, elementInformation)
        {
            
            Transform.Position = new Vector2(100,100);
            Transform.Size = new Vector2(200, 200);
            
            Transform.SetPositionSize(new Vector2(50,50), new Vector2(100,150));
        }
        
        public override void Instantiate()
        {
            OperationManager.GetOperation<StyleOperator>();
            
        }
    }
}