using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Management.OperationHandling
{
  
    public class StyleOperator : OperationBase
    {
        public StyleOperator StyleOp { get; init; }
        public StyleOperator(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            
        }
        
        public override void MakeOperator()
        {
            Console.WriteLine(nameof(StyleOperator));
        }

        private void SetStyle(string id, string styleKey, string value)
        {
            JsRuntime.InvokeVoidAsync("SetStyles", id, styleKey, value);
        }

        public void SetStyle(string id, params (string key, string val)[] args)
        {
            foreach ((string key,string val) in args)
            {
                SetStyle(id, key, val);
            }
        }
    }
}