using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Classes
{
    public interface IRenderable
    {
        public void RenderOutline(RenderTreeBuilder builder);
    }
}