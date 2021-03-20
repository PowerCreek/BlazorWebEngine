using System;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Management.OperationHandling
{
    public class OperationBase : IMakeOperator
    {
        public IJSRuntime JsRuntime { get; init; }

        public OperationBase(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }
        
        public virtual void MakeOperator()
        {
            Console.WriteLine(nameof(OperationBase));
        }
    }
}