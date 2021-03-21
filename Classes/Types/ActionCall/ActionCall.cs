using System;

namespace BlazorWebEngine.Classes.Types.ActionCall
{
    public abstract class ActionCall
    {
        public abstract void SetParameters(object a, object b);
    }

    public class ActionCall<T, V> : ActionCall where T : class
    {
        public Action<T, V> Action = (t, v) => { };

        public T Source { get; set; }
        public V Value { get; set; }

        public void Invoke(T a, V b)
        {
            SetParameters(a, b);
            Action(a, b);
        }

        public override void SetParameters(object a, object b)
        {
            Source = (T) a;
            Value = (V) b;
        }
    }
}