﻿using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Experimental;
using NUnit.Framework;
using System;

namespace Childrens_Social_Care_CPD_Tests.Contentful
{
    [TestFixture]
    public class EntityResolverTests
    {
        [Test]
        [TestCase("columnLayout", typeof(ColumnLayout))]
        [TestCase("content", typeof(Content))]
        [TestCase("contentLink", typeof(ContentLink))]
        [TestCase("contentSeparator", typeof(ContentSeparator))]
        [TestCase("detailedRole", typeof(DetailedRole))]
        [TestCase("heroBanner", typeof(HeroBanner))]
        [TestCase("linkCard", typeof(LinkCard))]
        [TestCase("linkListCard", typeof(LinkListCard))]
        [TestCase("richTextBlock", typeof(RichTextBlock))]
        [TestCase("roleList", typeof(RoleList))]
        [TestCase("sideMenu", typeof(SideMenu))]
        [TestCase("textBlock", typeof(TextBlock))]
        public void Resolves_Correctly(string contentTypeId, Type expectedType)
        { 
            var resolver = new EntityResolver();

            var actual = resolver.Resolve(contentTypeId);

            Assert.AreEqual(expectedType, actual);
        }

        [Test]
        [TestCase("", null)]
        [TestCase(null, null)]
        [TestCase("doesNotExist", null)]
        public void Does_Not_Resolve_Unknown_Content(string contentTypeId, Type expectedType)
        {
            var resolver = new EntityResolver();

            var actual = resolver.Resolve(contentTypeId);

            Assert.AreEqual(expectedType, actual);
        }
    }
}
