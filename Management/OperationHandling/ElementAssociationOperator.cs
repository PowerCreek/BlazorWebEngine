using System;
using BlazorWebEngine.Management.ElementManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebEngine.Management.OperationHandling
{
    public class ElementAssociationOperator : OperationBase
    {
        
        public ElementAssociationOperator(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            
        }

        public void SetParent(ElementReference parent, ElementReference child)
        {
            if (parent.Id == "" || child.Id == "") return;
            JsRuntime.InvokeVoidAsync("SetParent", parent, child);
        }
    }
}