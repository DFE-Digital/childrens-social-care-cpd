using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful;

public static class PartialsFactory
{
    /// <summary>
    /// Maps the POCO models to their respective Partial Views.
    /// </summary>
    /// <param name="item">An IContent instance</param>
    /// <returns>The partial view used to render that object.</returns>
    public static string GetPartialFor(IContent item)
    {
        switch (item)
        {
            case (ColumnLayout): return "./_ColumnLayout";
            case (Content): return "./_Content";
            case (ContentLink): return "./_ContentLink";
            case (ContentSeparator): return "./_ContentSeparator";
            case (DetailedRole): return "./_DetailedRole";
            case (HeroBanner): return string.Empty; // skip - handled in pre-render section
            case (LinkCard): return "./_LinkCard";
            case (LinkListCard): return "./_LinkListCard";
            case (RichTextBlock): return "./_RichTextBlock";
            case (RoleList): return "./_RoleList";
            case (SideMenu): return "./_SideMenu";
            case (TextBlock): return "./_TextBlock";
            default: return "./_UnknownContentWarning";
       }
    }
}
