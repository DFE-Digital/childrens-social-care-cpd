using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using System.IO;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class HorizontalRulerRendererTests
{
    private HorizontalRulerRenderer _sut = new HorizontalRulerRenderer();

    [Test]
    public void HorizontalRuler_Renders()
    {
        // arrange
        var stringWriter = new StringWriter();
        var horizontalRuler = new HorizontalRuler();

        // act
        var htmlContent = _sut.Render(horizontalRuler);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be("<hr class=\"HtmlEncode[[govuk-section-break govuk-section-break--m govuk-section-break--visible]]\" />");
    }
}
