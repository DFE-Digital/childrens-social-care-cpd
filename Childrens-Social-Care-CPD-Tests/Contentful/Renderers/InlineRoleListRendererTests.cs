using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class InlineRoleListRendererTests
{
    private IRenderer<ContentLink> _contentLinkRenderer;
    private InlineRoleListRenderer _sut;

    [SetUp]
    public void Setup()
    { 
        _contentLinkRenderer = Substitute.For<IRenderer<ContentLink>>();
        _sut = new InlineRoleListRenderer(_contentLinkRenderer);
    }

    [Test]
    public void RoleList_Renders_When_No_Roles()
    {
        // arrange
        var stringWriter = new StringWriter();
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>()
        };
        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<span>HtmlEncode[[No Roles Available]]</span>");
    }

    [Test]
    public void RoleList_Renders_Empty_When_DetailedRole_Is_Null()
    {
        // arrange
        var stringWriter = new StringWriter();
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Title = "title",
                    Items = new List<IContent>()
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void RoleList_Renders_Title()
    {
        // arrange
        var stringWriter = new StringWriter();
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        new DetailedRole()
                        {
                            Title = "title"
                        }
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        var expected = $"^{Regex.Escape("<div class=\"HtmlEncode[[govuk-heading-s govuk-!-margin-bottom-1]]\"><h3>AAA</h3></div>")}.*";

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void RoleList_Renders_Summary()
    {
        // arrange
        var stringWriter = new StringWriter();
        var detailedRole = new DetailedRole()
        {
            Title = "title",
            RoleListSummary = "summary"
        };
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        detailedRole
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));
        var expected = $".*?{Regex.Escape("<p class=\"HtmlEncode[[govuk-body]]\">HtmlEncode[[summary]]</p>")}.*";

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void RoleList_Renders_Salary_Range()
    {
        // arrange
        var stringWriter = new StringWriter();
        var detailedRole = new DetailedRole()
        {
            Title = "title",
            Summary = "summary",
            SalaryRange = "salary range"
        };
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        detailedRole
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        var sb = new StringBuilder();
        sb.Append("<div class=\"HtmlEncode[[govuk-grid-row govuk-!-padding-bottom-3]]\">");
        sb.Append("<div class=\"HtmlEncode[[govuk-grid-column-one-half]]\">");
        sb.Append("<ul class=\"HtmlEncode[[govuk-list]]\">");
        sb.Append("<li>");
        sb.Append($"<span class=\"HtmlEncode[[govuk-!-font-size-16 govuk-!-font-weight-bold]]\">HtmlEncode[[Salary range: ]]HtmlEncode[[{detailedRole.SalaryRange}]]</span>");
        sb.Append("</li>");
        sb.Append("</ul>");
        sb.Append("</div>");
        sb.Append("</div>");
        var expected = $".*?{Regex.Escape(sb.ToString())}.*";

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
    }

    [Test]
    public void RoleList_Does_Not_Renders_Salary_Range_For_Empty_value()
    {
        // arrange
        var stringWriter = new StringWriter();
        var detailedRole = new DetailedRole()
        {
            Title = "title",
            Summary = "summary",
            SalaryRange = ""
        };
        var roleList = new RoleList()
        {
            Title = "A Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "id",
                    Items = new List<IContent>()
                    {
                        detailedRole
                    }
                }
            }
        };

        _contentLinkRenderer.Render(Arg.Any<ContentLink>()).Returns(new HtmlString("AAA"));

        var sb = new StringBuilder();
        sb.Append("<div class=\"HtmlEncode[[govuk-heading-s govuk-!-margin-bottom-1]]\">");
        sb.Append("<h3>AAA</h3></div>");
        sb.Append("<p class=\"HtmlEncode[[govuk-body]]\"></p>");
        var expected = $".*?{Regex.Escape(sb.ToString())}.*";

        // act
        var htmlContent = _sut.Render(roleList);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().MatchRegex(expected);
        actual.Should().NotContain("Salary range:");
    }
}
