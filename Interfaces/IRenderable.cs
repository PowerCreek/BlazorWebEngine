﻿using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorWebEngine.Interfaces
{
    public interface IRenderable
    {
        public void RenderOutline(RenderTreeBuilder builder);
    }
}