using System;

namespace BlazorWebEngine.Management.OperationHandling
{
    public class OperationBase : IMakeOperator
    {
        public virtual void MakeOperator()
        {
            Console.WriteLine(nameof(OperationBase));
        }
    }
}