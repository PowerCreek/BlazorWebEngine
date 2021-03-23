namespace BlazorWebEngine.Components
{
    public class HtmlEntity
    {
        public HtmlEntity(string elementType)
        {
            ElementType = elementType;
        }

        public string ElementType { get; init; }
    }
}