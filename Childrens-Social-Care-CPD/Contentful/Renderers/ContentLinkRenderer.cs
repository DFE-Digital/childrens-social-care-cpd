using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Childrens_Social_Care_CPD.Contentful.Renderers;

interface IContentLinkContext
{
    string Path { get; }
    bool IsPromoBanner {get;}
}

internal class ContentLinkRenderer(IContentLinkContext contentLinkContext) : IRendererWithOptions<ContentLink>
{
    public IHtmlContent Render(ContentLink item, RendererOptions options = null)
    {
        var tagBuilder = new TagBuilder("a");

        var href = item.Uri switch
        {
            string s when s.StartsWith("http") => s,
            string s when s.StartsWith('/') => s,
            _ => string.Format("/{0}", item.Uri)
        };

        tagBuilder.Attributes.Add("href", href);
        tagBuilder.AddCssClass("govuk-link");

        if (options?.HasCss ?? false)
        {
            tagBuilder.AddCssClass(options.Css);
        }

        tagBuilder.Attributes.Add("data-track-label", contentLinkContext.Path);

        if (contentLinkContext.IsPromoBanner)
        {
            tagBuilder.AddCssClass("content-link--in-promo-banner");
        }



        //Define SVGs
        string svg = "";
        string linkText = item.Name;

        // Add different styling if link has an icon
        if (!string.IsNullOrEmpty(item.Icon))
        {
            tagBuilder.AddCssClass("content-link--with-icon");
        }

        switch (item.Icon) {
            case ContentLinkIcon.SignpostIcon:
                svg = "<svg aria-label=\"signpost icon\" class=\"svg-inline--fa\" aria-hidden=\"true\" focusable=\"false\" data-prefix=\"fas\" data-icon=\"signpost\" role=\"img\" xmlns=\"http://www.w3.org/2000/svg\" style=\"margin-right:10px\" width=\"40\" height=\"40\" viewBox=\"0 0 40 40\" fill=\"none\" data-fa-i2svg=\"\"><path fill-rule=\"evenodd\" clip-rule=\"evenodd\" d=\"M21.9938 1.99993C21.9938 0.895398 21.0984 0 19.9939 0C18.8893 0 17.9939 0.895399 17.9939 1.99993V3.99555H10.0023C8.89772 3.99555 8.00232 4.89094 8.00232 5.99547V13.9952C8.00232 15.0997 8.89772 15.9951 10.0023 15.9951H17.9939V17.9976L5.73313 17.9976C5.17879 17.9976 4.64931 18.2277 4.27107 18.633L0.53787 22.6328C-0.17929 23.4012 -0.17929 24.5936 0.53787 25.362L4.27107 29.3619C4.64931 29.7671 5.17879 29.9972 5.73313 29.9972L17.9939 29.9972V35.9996H12.0022C9.79312 35.9996 8.00232 37.7904 8.00232 39.9995H32.0015C32.0015 37.7904 30.2107 35.9996 28.0016 35.9996H21.9938V29.9972H29.9989C31.1035 29.9972 31.9989 29.1018 31.9989 27.9973V19.9976C31.9989 18.893 31.1035 17.9976 29.9989 17.9976H21.9938V15.9951H34.2681C34.8224 15.9951 35.3519 15.765 35.7301 15.3598L39.4633 11.3599C40.1805 10.5915 40.1805 9.39913 39.4633 8.63074L35.7301 4.63089C35.3519 4.22563 34.8224 3.99555 34.2681 3.99555H21.9938V1.99993ZM6.6022 21.9975L27.999 21.9975V25.9974L6.6022 25.9974L4.7356 23.9974L6.6022 21.9975ZM12.0022 7.9954V11.9953H33.399L35.2656 9.99533L33.399 7.9954H12.0022Z\" fill=\"#1D70B8\"></path></svg>";
            break;

            case ContentLinkIcon.CompassIcon:
                svg = "<svg aria-label=\"compass icon\" class=\"svg-inline--fa\" aria-hidden=\"true\" focusable=\"false\" data-prefix=\"fas\" data-icon=\"compass\" role=\"img\" xmlns=\"http://www.w3.org/2000/svg\" style=\"margin-right:10px\" width=\"40\" height=\"40\" viewBox=\"0 0 40 40\" fill=\"none\" data-fa-i2svg=\"\"><path fill-rule=\"evenodd\" clip-rule=\"evenodd\" d=\"M20 36C11.1634 36 4 28.8366 4 20C4 11.1634 11.1634 4 20 4C28.8366 4 36 11.1634 36 20C36 28.8366 28.8366 36 20 36ZM0 20C0 31.0457 8.95431 40 20 40C31.0457 40 40 31.0457 40 20C40 8.9543 31.0457 0 20 0C8.9543 0 0 8.95431 0 20ZM9.0026 21.9987C10.1072 21.9987 11.0026 21.1033 11.0026 19.9987C11.0026 18.8941 10.1072 17.9987 9.0026 17.9987H8.0026C6.89804 17.9987 6.0026 18.8941 6.0026 19.9987C6.0026 21.1033 6.89804 21.9987 8.0026 21.9987H9.0026ZM33.9974 19.9987C33.9974 21.1033 33.102 21.9987 31.9974 21.9987H30.9974C29.8928 21.9987 28.9974 21.1033 28.9974 19.9987C28.9974 18.8941 29.8928 17.9987 30.9974 17.9987H31.9974C33.102 17.9987 33.9974 18.8941 33.9974 19.9987ZM21.9987 30.9974C21.9987 29.8928 21.1033 28.9974 19.9987 28.9974C18.8941 28.9974 17.9987 29.8928 17.9987 30.9974V31.9974C17.9987 33.102 18.8941 33.9974 19.9987 33.9974C21.1033 33.9974 21.9987 33.102 21.9987 31.9974V30.9974ZM19.9987 6.0026C21.1033 6.0026 21.9987 6.89803 21.9987 8.0026V9.0026C21.9987 10.1072 21.1033 11.0026 19.9987 11.0026C18.8941 11.0026 17.9987 10.1072 17.9987 9.0026V8.0026C17.9987 6.89803 18.8941 6.0026 19.9987 6.0026ZM20.5565 24.5641C22.2448 23.5881 23.6667 22.1662 24.6427 20.4779L30.0324 11.1552C30.521 10.31 29.688 9.47696 28.8428 9.96559L19.5201 15.3553C17.8318 16.3313 16.4099 17.7532 15.4339 19.4415L10.0442 28.7642C9.55558 29.6094 10.3886 30.4424 11.2338 29.9538L20.5565 24.5641ZM21.4142 21.4093C20.6332 22.1904 19.3668 22.1904 18.5858 21.4093C17.8047 20.6283 17.8047 19.3619 18.5858 18.5809C19.3668 17.7998 20.6332 17.7998 21.4142 18.5809C22.1953 19.3619 22.1953 20.6283 21.4142 21.4093Z\" fill=\"#1D70B8\"></path></svg>";
            break;
        }

        if (string.IsNullOrEmpty(svg))
        {
            tagBuilder.InnerHtml.Append(linkText);
        }
        
        else 
        {
            var innerHtml = new HtmlContentBuilder();
            innerHtml.AppendHtml(svg);
            linkText = "<span class=\"content-link-text\">" + linkText + "</span>";
            innerHtml.AppendHtml(linkText);
            tagBuilder.InnerHtml.SetHtmlContent(innerHtml);
        }

        return tagBuilder;
    }

    public IHtmlContent Render(ContentLink item)
    {
        return Render(item, null);
    }

}
