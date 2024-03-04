using Childrens_Social_Care_CPD;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Childrens_Social_Care_CPD_Tests;

public class CustomHtmlHelpersForScriptsTests
{
    private IHtmlHelper _htmlHelper;
    private IDictionary<object, object> _items;

    [SetUp]
    public void Setup()
    {
        _items = new Dictionary<object, object>();
        var httpContext = Substitute.For<HttpContext>();
        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        _htmlHelper = Substitute.For<IHtmlHelper>();
        _htmlHelper.ViewContext.Returns(viewContext);
        httpContext.Items.Returns(_items);

        var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        urlHelperFactory.GetUrlHelper(Arg.Any<ActionContext>()).Returns(new UrlHelper(actionContext));
        httpContext.RequestServices.GetService(typeof(IUrlHelperFactory)).Returns(urlHelperFactory);
    }

    [Test]
    public void RequireScriptUrl_Throws_Without_Helper()
    {
        // act/arrange
        Action act = () => CustomHtmlHelpers.RequireScriptUrl(null, "/script.js");

        // assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestCase("script.js")]
    [TestCase("/script.js")]
    [TestCase("~/script.js")]
    [TestCase("https://www.google.com/script.js")]
    public void RequireScriptUrl_Stores_Script_With_Url(string scriptUrl)
    {
        // act/arrange
        _htmlHelper.RequireScriptUrl(scriptUrl);
        var scripts = _items["CustomScripts"] as IDictionary<string, ScriptInfo>;

        // assert
        scripts.Should().ContainKey(scriptUrl);
    }

    [TestCase("")]
    [TestCase(null)]
    public void RequireScriptUrl_Does_Not_Store_Script_With_No_Url(string url)
    {
        // act/arrange
        _htmlHelper.RequireScriptUrl(url);

        // assert
        _items.Should().BeEmpty();
    }

    [Test]
    public void RequireScriptUrl_Stores_Script_With_Details()
    {
        // arrange
        var scriptUrl = "/script.js";

        //act
        _htmlHelper.RequireScriptUrl(scriptUrl, asynchronous: true, defer: true, ScriptPosition.BodyStart);
        var scripts = _items["CustomScripts"] as IDictionary<string, ScriptInfo>;
        var scriptInfo = scripts[scriptUrl];
        
        // assert
        scriptInfo.Should().NotBeNull();
        scriptInfo.Source.Should().Be(scriptUrl);
        scriptInfo.Asynchronous.Should().BeTrue();
        scriptInfo.Defer.Should().BeTrue();
        scriptInfo.Position.Should().Be(ScriptPosition.BodyStart);
    }

    [Test]
    public void RenderScripts_Throws_Without_Helper()
    {
        // act/arrange
        Action act = () => CustomHtmlHelpers.RenderScripts(null, ScriptPosition.BodyEnd);

        // assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void RenderScripts_Should_Render_Nothing_Without_Any_Scripts()
    {
        // act/arrange
        var actual = _htmlHelper.RenderScripts(ScriptPosition.BodyEnd);

        // assert
        actual.Should().Be(HtmlString.Empty);
    }

    [Test]
    public void RenderScripts_Should_Render_Script_Tags()
    {
        // act
        _htmlHelper.RequireScriptUrl("/script.js", false, false, ScriptPosition.BodyEnd);
        
        // arrange
        var actual = _htmlHelper.RenderScripts(ScriptPosition.BodyEnd);

        // assert
        actual.ToString().Should().StartWith("<script src=\"/script.js\"></script>");
    }

    [Test]
    public void RenderScripts_Should_Not_Render_Script_Not_In_Specified_Section()
    {
        // act
        _htmlHelper.RequireScriptUrl("/script.js", false, false, ScriptPosition.BodyEnd);

        // arrange
        var actual = _htmlHelper.RenderScripts(ScriptPosition.BodyStart);

        // assert
        actual.ToString().Should().Be(string.Empty);
    }
}