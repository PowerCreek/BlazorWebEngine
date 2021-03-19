using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using BlazorWebEngine.Annotations;
using BlazorWebEngine.Management;

namespace BlazorWebEngine.Classes
{
    
    public interface IChangedProperty<T>
    {
        public delegate void ElementPropertyChangedHandler(object sender, ElementArgs<T> e);
        public event ElementPropertyChangedHandler PropertyChanged;
        public void OnPropertyChanged(ElementProperty<T> self, object value);
    }

    public class ElementProperty<T> : IChangedProperty<T>
    {
        public event IChangedProperty<T>.ElementPropertyChangedHandler PropertyChanged;
        public virtual void OnPropertyChanged(ElementProperty<T> self, object value)
        {
            string name = new StackTrace().GetFrame(1)?.GetMethod()?.Name.Split('_')[1];
            PropertyChanged?.Invoke(this, new ElementArgs<T>(name, self, value));
        }
    }

    public class ElementArgs<T> : PropertyChangedEventArgs
    {
        public T Arguments { get; init; }
        
        public readonly dynamic Value;
        
        public ElementArgs(string name, object arguments, object value) : base(name)
        {
            Arguments = (T) arguments;
            Value = value;
        }

        public T GetArgs()
        {
            return Arguments;
        }
    }

    public class TransformArgs : ElementArgs<Transform>
    {
        
        public TransformArgs(string name, Transform elementProperty, object value) : base(name, elementProperty, value)
        {
        }
    }
    
    public class Transform : ElementProperty<Transform>
    {
        private Vector2 _position;
        private Vector2 _size;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(this, value);
            }
        }

        public Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged(this, value);
            }
        }
        
    }
}