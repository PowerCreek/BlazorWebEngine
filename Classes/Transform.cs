using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BlazorWebEngine.Classes
{
    public class Transform
    {
        private Vector2 _position = Vector2.Zero;
        private Vector2 _size;
        public Vector2 Position
        {
            get => _position;
            set
            {
                OnPositionStyleSet(value);
                _position = value;
                OnPosition(value);
            }
        }

        public Vector2 Size
        {
            get => _size;
            set
            {
                OnSizeStyleSet(value);
                _size = value;
                OnSize(value);
            }
        }

        public Action<Vector2> OnPosition = (a) => { };
        public Action<Vector2> OnSize = (a) => { };

        public Action<Vector2> OnPositionStyleSet = (vec) => { };
        public Action<Vector2> OnSizeStyleSet = (vec) => { };

    }
}