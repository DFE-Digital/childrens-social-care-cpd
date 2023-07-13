using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Configuration;

namespace Childrens_Social_Care_CPD.Experimental;

/// <summary>
/// Resolves ContentTypeId strings to their POCO representations.
/// Used by the Contentful client side library to serialize JSON objects from the response into our Models.
/// </summary>
public class EntityResolver : IContentTypeResolver
{
    public Dictionary<string, Type> _types = new Dictionary<string, Type>()
    {
        { "columnLayout", typeof(ColumnLayout) },
        { "content", typeof(Content) },
        { "contentLink", typeof(ContentLink) },
        { "contentSeparator", typeof(ContentSeparator) },
        { "detailedRole", typeof(DetailedRole) },
        { "heroBanner", typeof(HeroBanner) },
        { "linkCard", typeof(LinkCard) },
        { "linkListCard", typeof(LinkListCard) },
        { "richTextBlock", typeof(RichTextBlock) },
        { "roleList", typeof(RoleList) },
        { "sideMenu", typeof(SideMenu) },
        { "textBlock", typeof(TextBlock) },
    };

    public Type Resolve(string contentTypeId)
    {
        if (contentTypeId == null)
            return null;

        _types.TryGetValue(contentTypeId, out var type);
        return type;
    }
}

