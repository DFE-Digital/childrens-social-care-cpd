﻿using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

[TestFixture]
public class EntityResolverTests
{
    [Test]
    [TestCase("accordion", typeof(Accordion))]
    [TestCase("accordionSection", typeof(AccordionSection))]
    [TestCase("applicationFeature", typeof(ApplicationFeature))]
    [TestCase("applicationFeatures", typeof(ApplicationFeatures))]
    [TestCase("areaOfPractice", typeof(AreaOfPractice))]
    [TestCase("areaOfPracticeList", typeof(AreaOfPracticeList))]
    [TestCase("assetDownload", typeof(AssetDownload))]
    [TestCase("audioResource", typeof(AudioResource))]
    [TestCase("backToTop", typeof(BackToTop))]
    [TestCase("columnLayout", typeof(ColumnLayout))]
    [TestCase("content", typeof(Content))]
    [TestCase("contentLink", typeof(ContentLink))]
    [TestCase("contentsAnchor", typeof(ContentsAnchor))]
    [TestCase("contentSeparator", typeof(ContentSeparator))]
    [TestCase("creditBlock", typeof(CreditBlock))]
    [TestCase("detailedPathway", typeof(DetailedPathway))]
    [TestCase("detailedRole", typeof(DetailedRole))]
    [TestCase("details", typeof(Details))]
    [TestCase("feedback", typeof(Feedback))]
    [TestCase("heroBanner", typeof(HeroBanner))]
    [TestCase("imageCard", typeof(ImageCard))]
    [TestCase("linkCard", typeof(LinkCard))]
    [TestCase("linkListCard", typeof(LinkListCard))]
    [TestCase("pageContents", typeof(PageContents))]
    [TestCase("pageContentsItem", typeof(PageContentsItem))]
    [TestCase("pathwaysModule", typeof(PathwaysModule))]
    [TestCase("pathwaysModuleSection", typeof(PathwaysModuleSection))]
    [TestCase("pdfFileResource", typeof(PdfFileResource))]
    [TestCase("quoteBox", typeof(QuoteBox))]
    [TestCase("richTextBlock", typeof(RichTextBlock))]
    [TestCase("roleList", typeof(RoleList))]
    [TestCase("navigationMenu", typeof(NavigationMenu))]
    [TestCase("textBlock", typeof(TextBlock))]
    [TestCase("videoResource", typeof(VideoResource))]
    [TestCase("infoBox", typeof(InfoBox))]
    public void Resolves_Correctly(string contentTypeId, Type expectedType)
    { 
        var resolver = new EntityResolver();

        var actual = resolver.Resolve(contentTypeId);

        actual.Should().Be(expectedType);
    }

    [Test]
    [TestCase("", null)]
    [TestCase(null, null)]
    [TestCase("doesNotExist", null)]
    public void Does_Not_Resolve_Unknown_Content(string contentTypeId, Type expectedType)
    {
        var resolver = new EntityResolver();

        var actual = resolver.Resolve(contentTypeId);

        actual.Should().Be(expectedType);
    }
}
