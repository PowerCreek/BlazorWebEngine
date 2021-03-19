namespace BlazorWebEngine.Classes
{
    public interface IChangedProperty<T, V>
    {
        public delegate void ElementPropertyChangedHandler(string name, T t, V v);
    }
}