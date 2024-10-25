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
        return item switch
        {
            Accordion => "_Accordion",
            AccordionSection => "_AccordionSection",
            AreaOfPractice => "_AreaOfPractice",
            AreaOfPracticeList => "_AreaOfPracticeList",
            AssetDownload => "_AssetDownload",
            AudioResource => "_AudioResource",
            BackToTop => "_BackToTop",
            ColumnLayout => "_ColumnLayout",
            Content => "_Content",
            ContentLink => "_ContentLink",
            ContentSeparator => "_ContentSeparator",
            ContentsAnchor => "_ContentsAnchor",
            CreditBlock => "_CreditBlock",
            DetailedRole => "_DetailedRole",
            DetailedPathway => "_DetailedPathway",
            Details => "_Details",
            Feedback => "_Feedback",
            HeroBanner => string.Empty,// skip - handled in specific layout section
            ImageCard => "_ImageCard",
            LinkCard => "_LinkCard",
            LinkListCard => "_LinkListCard",
            PageContents => "_PageContents",
            PageContentsItem => "_PageContentsItem",
            PdfFileResource => "_PdfFileResource",
            RichTextBlock => "_RichTextBlock",
            RoleList => "_RoleList",
            NavigationMenu => "_NavigationMenu",
            TextBlock => "_TextBlock",
            VideoResource => "_VideoResource",
            InfoBox => "_InfoBox",
            _ => "_UnknownContentWarning",
        };
    }
}
