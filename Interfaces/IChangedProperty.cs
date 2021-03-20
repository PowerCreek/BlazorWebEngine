namespace BlazorWebEngine.Interfaces
{
    public interface IChangedProperty<T, V>
    {
        public delegate void ElementPropertyChangedHandler(string name, T t, V v);
    }
}