using Microsoft.AspNetCore.Html;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public record RendererOptions(string Css = default)
{
    public bool HasCss => Css != default;
}

public interface IRenderer<in T>
{
    IHtmlContent Render(T item);
}

public interface IRendererWithOptions<in T> : IRenderer<T>
{
    IHtmlContent Render(T item, RendererOptions options = null);
}