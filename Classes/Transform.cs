using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using BlazorWebEngine.Annotations;
using BlazorWebEngine.Management;

namespace BlazorWebEngine.Classes
{
    public class Transform
    {
        private Vector2 _position;
        private Vector2 _size;
        
        public Vector2 Position
        {
            get => _position;
            set => OnMove?.Invoke(nameof(Position),this, _position=value);
        }

        public Vector2 Size
        {
            get => _size;
            set => OnResize?.Invoke(nameof(Size),this, _size=value);
        }

        public void SetPositionSize(int x, int y, int w, int h)
        {
            SetPositionSize(new Vector2(x, y),new Vector2(w,h));
        }

        public void SetPositionSize(Vector2 position, Vector2 size)
        {
            _position = position;
            Size = size;
        }

        public IChangedProperty<Transform, Vector2>.ElementPropertyChangedHandler OnMove =
            (string s, Transform t, Vector2 value) =>
            {

            };
        
        public IChangedProperty<Transform, Vector2>.ElementPropertyChangedHandler OnResize =
            (string s, Transform t, Vector2 value) =>
            {

            };
    }
}