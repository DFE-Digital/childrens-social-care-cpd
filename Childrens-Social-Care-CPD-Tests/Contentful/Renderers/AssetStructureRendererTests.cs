
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.WebEncoders.Testing;
using NUnit.Framework;
using StringWriter = System.IO.StringWriter;


namespace Childrens_Social_Care_CPD_Tests.Contentful.Renderers;

public class AssetStructureRendererTests
{
    private readonly AssetStructureRenderer _sut = new AssetStructureRenderer();

    [Test]
    public void Ignores_Empty_Structure()
    {
        // arrange
        var assetStructure = new AssetStructure();

        // act
        var htmlContent = _sut.Render(assetStructure);
        
        // assert
        htmlContent.Should().BeNull();
    }

    [Test]
    public void Ignores_Empty_StructureData()
    {
        // arrange
        var assetStructure = new AssetStructure
        {
            Data = new AssetStructureData()
        };

        // act
        var htmlContent = _sut.Render(assetStructure);

        // assert
        htmlContent.Should().BeNull();
    }

    [Test]
    public void Ignores_Empty_Asset()
    {
        // arrange
        var assetStructure = new AssetStructure
        {
            Data = new AssetStructureData
            {
                Target = new Asset()
            }
        };

        // act
        var htmlContent = _sut.Render(assetStructure);

        // assert
        htmlContent.Should().BeNull();
    }

    [TestCase("text/html")]
    [TestCase("application/pdf")]
    public void Ignores_ContentTypes(string contentType)
    {
        // arrange
        var assetStructure = new AssetStructure
        {
            Data = new AssetStructureData
            {
                Target = new Asset
                {
                    File = new File
                    {
                        ContentType = contentType,
                        Url = "/foo"
                    },
                    Description = "foo"
                }
            }
        };

        // act
        var htmlContent = _sut.Render(assetStructure);

        // assert
        htmlContent.Should().BeNull();
    }

    [TestCase("image/png")]
    [TestCase("Image/PNG")]
    [TestCase("image/gif")]
    [TestCase("image/xxx")]
    public void Renders_Image_Asset(string contentType)
    {
        // arrange
        var stringWriter = new StringWriter();
        var assetStructure = new AssetStructure
        {
            Data = new AssetStructureData
            {
                Target = new Asset
                {
                    File = new File
                    {
                        ContentType = contentType,
                        Url = "/foo"
                    },
                    Description = "foo"
                }
            }
        };

        // act
        var htmlContent = _sut.Render(assetStructure);
        htmlContent.WriteTo(stringWriter, new HtmlTestEncoder());
        var actual = stringWriter.ToString();

        // assert
        actual.Should().Be($"<img alt=\"HtmlEncode[[foo]]\" src=\"HtmlEncode[[/foo]]\"></img>");
    }
}
