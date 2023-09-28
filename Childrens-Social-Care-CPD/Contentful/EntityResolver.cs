﻿using Childrens_Social_Care_CPD.Contentful.Models;
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
            "areaOfPractice" => typeof(AreaOfPractice),
            "areaOfPracticeList" => typeof(AreaOfPracticeList),
            "applicationFeature" => typeof(ApplicationFeature),
            "applicationFeatures" => typeof(ApplicationFeatures),
            "columnLayout" => typeof(ColumnLayout),
            "content" => typeof(Content),
            "contentLink" => typeof(ContentLink),
            "contentSeparator" => typeof(ContentSeparator),
            "detailedPathway" => typeof(DetailedPathway),
            "detailedRole" => typeof(DetailedRole),
            "heroBanner" => typeof(HeroBanner),
            "imageCard" => typeof(ImageCard),
            "linkCard" => typeof(LinkCard),
            "linkListCard" => typeof(LinkListCard),
            "resource" => typeof(Resource),
            "richTextBlock" => typeof(RichTextBlock),
            "roleList" => typeof(RoleList),
            "sideMenu" => typeof(SideMenu),
            "textBlock" => typeof(TextBlock),
            "video" => typeof(Video),
            _ => null
        };
    }
}

