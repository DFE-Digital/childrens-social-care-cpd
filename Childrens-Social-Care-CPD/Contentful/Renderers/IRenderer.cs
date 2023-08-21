using Microsoft.AspNetCore.Html;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

public interface IRenderer<T>
{
    IHtmlContent Render(T item);
}
