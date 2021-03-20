using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using BlazorWebEngine.Properties;

namespace BlazorWebEngine.Management.ElementManagement.ElementProperties
{

    public abstract class ActionCall
    {

        public abstract void SetParameters(object a, object b);

    }
    
    public class ActionCall<T,V> : ActionCall where T: class 
    {

        public Action<T,V> Action = (t,v) => { };

        public T Source { get; set; }
        public V Value { get; set; }
        
        public void Invoke(T a, V b)
        {
            SetParameters(a,b);
            Action(a,b);
        }

        public override void SetParameters(object a, object b)
        {
            Source = (T)a;
            Value = (V) b;
        }
    } 
    
    public class ElementProperty<T> where T: class
    {

        public Dictionary<string, ActionCall> PropertyChangedMap = new();

        
        public ActionCall<T,V> GetPropertyActionCall<V>()
        {
            string name = new StackTrace().GetFrame(1)?.GetMethod()?.Name.Split('_')[1];
            if (PropertyChangedMap.TryGetValue(name, out var hold))
            {
                return hold as ActionCall<T,V>;
            }
            
            var action = Activator.CreateInstance<ActionCall<T,V>>();
            PropertyChangedMap.Add(name, action);
            
            return action;
        }
    }

    public class Vector : INotifyPropertyChanged, IEquatable<int[]>
    {
        protected int[] values = {0, 0};
        
        public Vector()
        {
        }
        public Vector(int a, int b)
        {
            values[0] = a;
            values[1] = b;
        }
        
        

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(int[] other)
        {
            return other != null && Enumerable.SequenceEqual(values, other);
        }
    }

    public class Position : Vector
    {
        public int X
        {
            get => values[0];
            set
            {
                if (values[0] != value)
                {
                    values[0] = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Y
        {
            get => values[1];
            set
            {
                if (values[1] == value) return;
                values[1] = value;
                OnPropertyChanged();
            }
        }
        
        public Position() : this(0,0)
        {
        }
        public Position(int x, int y) :base(x,y)
        {
        }
        
    }
    public class Size : Vector
    {
        public int Width
        {
            get => values[0];
            set
            {
                if (values[0] == value) return;
                values[0] = value;
                OnPropertyChanged();
            }
        }
        public int Height
        {
            get => values[1];
            set
            {
                if (values[1] == value) return;
                values[1] = value;
                OnPropertyChanged();
            }
        }

        public Size() : this(0, 0)
        {
            
        }
        
        public Size(int width, int height) :base(width,height)
        {
        }
    }
    
    
    public class Transform : ElementProperty<Transform>
    {
        
        public Transform() : base()
        {
            Position = new();
            Size = new();
        }
        
        private Size _size = new Size();
        public Size Size
        {
            get => _size;
            set
            {
                if (_size.Equals(value)) return;
                _size = value;
                _size.PropertyChanged += (a,b)=>
                {
                    OnResize?.Invoke(this, _size);
                };
                OnResize?.Invoke(this,_size);
            }
        }

        public Action<Transform,Size> OnResize
        {
            get => GetPropertyActionCall<Size>().Invoke;
            set => GetPropertyActionCall<Size>().Action+=value;
        }
        
        private Position _position = new Position();

        public Position Position
        {
            get => _position;
            set
            {
                if (_position.Equals(value)) return;
                _position = value;
                _position.PropertyChanged += (a,b)=>
                {
                    OnMove?.Invoke(this, _position);
                    Console.WriteLine("setting property of position");
                };
                OnMove?.Invoke(this, _position);
                Console.WriteLine("setting position");
            }
        }

        public Action<Transform, Position> OnMove
        {
            get => GetPropertyActionCall<Position>().Invoke;
            set => GetPropertyActionCall<Position>().Action=(value);
        }
        
        public void SetPositionSize(int x, int y, int w, int h) => SetPositionSize(new Position(x, y), new Size(w,h));

        public void SetPositionSize(Position position, Size size)
        {
            _position = position;
            Size = size;
        }
    }
}