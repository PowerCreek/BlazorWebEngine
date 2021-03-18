using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;

namespace BlazorWebEngine.Classes
{
    public interface IInstance
    {
        public void Instantiate(ElementManager elementManager);
    }
}