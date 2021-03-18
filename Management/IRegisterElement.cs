using BlazorWebEngine.Management.ElementHandling;

namespace BlazorWebEngine.Management
{
    public interface IRegisterElement
    {
        public int AddElement();
        public void RemoveElement(int id);
    }
}