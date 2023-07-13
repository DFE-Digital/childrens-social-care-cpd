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
            case (ColumnLayout): return "./ColumnLayout";
            case (Content): return "./Content";
            case (ContentLink): return "./ContentLink";
            case (ContentSeparator): return "./ContentSeparator";
            case (DetailedRole): return "./DetailedRole";
            case (HeroBanner): return string.Empty; // skip - handled in pre-render section
            case (LinkCard): return "./LinkCard";
            case (LinkListCard): return "./LinkListCard";
            case (RichTextBlock): return "./RichTextBlock";
            case (RoleList): return "./RoleList";
            case (SideMenu): return "./SideMenu";
            case (TextBlock): return "./TextBlock";
            default : return "./UnknownContentWarning";
        }
    }
}
