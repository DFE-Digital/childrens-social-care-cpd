using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Contentful.Core.Models;
using Contentful.Core.Search;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.DataAccess;

public class ContentRepositoryTests
{
    private ICpdContentfulClient _contentfulClient;

#pragma warning disable S1121 // Assignments should not be made from within sub-expressions

    private void SetContent(Content content)
    {
        var contentCollection = new ContentfulCollection<Content>();

        contentCollection.Items = content == null
            ? new List<Content>()
            : contentCollection.Items = new List<Content> { content };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(contentCollection);


    }

#pragma warning restore S1121 // Assignments should not be made from within sub-expressions

    [SetUp]
    public void SetUp()
    {
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
    }

    [Test]
    public async Task FetchPageContentAsync_Returns_Content()
    {
        // arrange
        var content = new Content();
        SetContent(content);

        // act
        var contentRepository = new ContentRepository(_contentfulClient);
        var actual = await contentRepository.FetchPageContentAsync("home", CancellationToken.None);

        // assert
        actual.Should().Be(content);
    }

    [Test]
    public async Task Index_Trims_Trailing_Slashes()
    {
        // arrange
        SetContent(new Content());
        var query = "";
        await _contentfulClient.GetEntries(Arg.Do<QueryBuilder<Content>>(value => query = value.Build()), Arg.Any<CancellationToken>());
        var contentRepository = new ContentRepository(_contentfulClient);

        // act
        await contentRepository.FetchPageContentAsync("home/", CancellationToken.None);
        

        // assert
        query.Should().Contain("?content_type=content&fields.id=home%2F&include=10");
    }
}
