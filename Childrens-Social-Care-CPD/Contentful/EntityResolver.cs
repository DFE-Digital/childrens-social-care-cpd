using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Configuration;

namespace Childrens_Social_Care_CPD.Contentful;

/// <summary>
/// Resolves ContentTypeId strings to their POCO representations.
/// Used by the Contentful client side library to serialize JSON objects from the response into our Models.
/// </summary>
public class EntityResolver : IContentTypeResolver
{
    public Type Resolve(string contentTypeId)
    {
        return contentTypeId switch
        {
            "accordion" => typeof(Accordion),
            "accordionSection" => typeof(AccordionSection),
            "applicationFeature" => typeof(ApplicationFeature),
            "applicationFeatures" => typeof(ApplicationFeatures),
            "areaOfPractice" => typeof(AreaOfPractice),
            "areaOfPracticeList" => typeof(AreaOfPracticeList),
            "assetDownload" => typeof(AssetDownload),
            "audioResource" => typeof(AudioResource),
            "backToTop" => typeof(BackToTop),
            "columnLayout" => typeof(ColumnLayout),
            "content" => typeof(Content),
            "contentLink" => typeof(ContentLink),
            "contentsAnchor" => typeof(ContentsAnchor),
            "contentSeparator" => typeof(ContentSeparator),
            "creditBlock" => typeof(CreditBlock),
            "detailedPathway" => typeof(DetailedPathway),
            "detailedRole" => typeof(DetailedRole),
            "details" => typeof(Details),
            "feedback" => typeof(Feedback),
            "heroBanner" => typeof(HeroBanner),
            "imageCard" => typeof(ImageCard),
            "linkCard" => typeof(LinkCard),
            "linkListCard" => typeof(LinkListCard),
            "pageContents" => typeof(PageContents),
            "pageContentsItem" => typeof(PageContentsItem),
            "pathwaysModule" => typeof(PathwaysModule),
            "pathwaysModuleSection" => typeof(PathwaysModuleSection),
            "pdfFileResource" => typeof(PdfFileResource),
            "quoteBox" => typeof(QuoteBox),
            "richTextBlock" => typeof(RichTextBlock),
            "roleList" => typeof(RoleList),
            "navigationMenu" => typeof(NavigationMenu),
            "textBlock" => typeof(TextBlock),
            "videoResource" => typeof(VideoResource),
            "infoBox" => typeof(InfoBox),
            _ => null
        };
    }
}

