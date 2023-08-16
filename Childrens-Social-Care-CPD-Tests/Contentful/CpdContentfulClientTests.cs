using Childrens_Social_Care_CPD.Contentful;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;

namespace Childrens_Social_Care_CPD_Tests.Contentful;

public class CpdContentfulClientTest
{
    private IContentTypeResolver _resolver;
    private IContentfulClient _contentfulClient;
    private ICpdContentfulClient _cpdClient;

    [SetUp]
    public void Setup()
    {
        // arrange
        _resolver = Substitute.For<IContentTypeResolver>();
        _contentfulClient = Substitute.For<IContentfulClient>();
        _cpdClient = new CpdContentfulClient(_contentfulClient, _resolver);
    }

    [Test]
    public void Assigns_EntityResolver_To_Base_Client()
    {
        // assert
        _contentfulClient.ContentTypeResolver.Should().Be(_resolver);
    }

    #region Properties

    [Test]
    public void Serializer_Getter_Was_Passed_Through()
    {
        // act
        _ = _cpdClient.Serializer;

        // assert
        _ = _contentfulClient.Received().Serializer;
    }

    [Test]
    public void IsPreviewClient_Getter_Was_Passed_Through()
    {
        // act
        _ = _cpdClient.IsPreviewClient;

        // assert
        _ = _contentfulClient.Received().IsPreviewClient;
    }

    [Test]
    public void ContentTypeResolver_Is_Passed_Through()
    {
        // arrange
        var contentTypeResolver = Substitute.For<IContentTypeResolver>();

        // act
        _cpdClient.ContentTypeResolver = contentTypeResolver;

        // assert
        _contentfulClient.ContentTypeResolver.Should().Be(contentTypeResolver);
    }

    [Test]
    public void SerializerSettings_Is_Passed_Through()
    {
        // arrange
        var serializerSettings = Substitute.For<JsonSerializerSettings>();

        // act
        _cpdClient.SerializerSettings = serializerSettings;

        // assert
        _contentfulClient.SerializerSettings.Should().Be(serializerSettings);
    }

    [Test]
    public void ResolveEntriesSelectively_Is_Passed_Through()
    {
        // act
        _ = _cpdClient.ResolveEntriesSelectively;

        // assert
        _ = _contentfulClient.Received().ResolveEntriesSelectively;
    }

    #endregion

    #region Methods

    [Test]
    public void Calls_Base_CreateEmbargoedAssetKey()
    {
        // act
        _cpdClient.CreateEmbargoedAssetKey(new DateTimeOffset());

        // assert
        _contentfulClient.Received().CreateEmbargoedAssetKey(Arg.Any<DateTimeOffset>());
    }

    [Test]
    public void Calls_Base_GetAsset()
    {
        // act
        _cpdClient.GetAsset(string.Empty, string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetAsset(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetAsset_Overload_1()
    {
        // act
        _cpdClient.GetAsset(string.Empty, new QueryBuilder<Asset>());

        // assert
        _contentfulClient.Received().GetAsset(Arg.Any<string>(), Arg.Any<QueryBuilder<Asset>>());
    }

    [Test]
    public void Calls_Base_GetAsset_Overload_2()
    {
        // act
        _cpdClient.GetAsset(string.Empty);

        // assert
        _contentfulClient.Received().GetAsset(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetAssets()
    {
        // act
        _cpdClient.GetAssets(string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetAssets(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetAssets_Overload_1()
    {
        // act
        _cpdClient.GetAssets(new QueryBuilder<Asset>());

        // assert
        _contentfulClient.Received().GetAssets(Arg.Any<QueryBuilder<Asset>>());
    }

    [Test]
    public void Calls_Base_GetAssets_Overload_2()
    {
        // act
        _cpdClient.GetAssets();

        // assert
        _contentfulClient.Received().GetAssets();
    }

    [Test]
    public void Calls_Base_GetContentType()
    {
        // act
        _cpdClient.GetContentType(string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetContentType(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetContentType_Overload()
    {
        // act
        _cpdClient.GetContentType(string.Empty);

        // assert
        _contentfulClient.Received().GetContentType(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetContentTypes()
    {
        // act
        _cpdClient.GetContentTypes(string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetContentTypes(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetContentTypes_Overload_1()
    {
        // act
        _cpdClient.GetContentTypes(new CancellationToken());

        // assert
        _contentfulClient.Received().GetContentTypes(Arg.Any<CancellationToken>());
    }

    [Test]
    public void Calls_Base_GetContentTypes_Overload_2()
    {
        // act
        _cpdClient.GetContentTypes(string.Empty);

        // assert
        _contentfulClient.Received().GetContentTypes(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetEntries()
    {
        // act
        _cpdClient.GetEntries(new QueryBuilder<TestingContentItem>());

        // assert
        _contentfulClient.Received().GetEntries(Arg.Any<QueryBuilder<TestingContentItem>>());
    }

    [Test]
    public void Calls_Base_GetEntries_Overload_1()
    {
        // act
        _cpdClient.GetEntries<TestingContentItem>(string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetEntries<TestingContentItem>(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetEntries_Overload_2()
    {
        // act
        _cpdClient.GetEntries<TestingContentItem>();

        // assert
        _contentfulClient.Received().GetEntries<TestingContentItem>();
    }

    [Test]
    public void Calls_Base_GetEntriesByType()
    {
        // act
        _cpdClient.GetEntriesByType(string.Empty, new QueryBuilder<TestingContentItem>());

        // assert
        _contentfulClient.Received().GetEntriesByType(Arg.Any<string>(), Arg.Any<QueryBuilder<TestingContentItem>>());
    }

    [Test]
    public void Calls_Base_GetEntriesRaw()
    {
        // act
        _cpdClient.GetEntriesRaw(string.Empty);

        // assert
        _contentfulClient.Received().GetEntriesRaw(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetEntry()
    {
        // act
        _cpdClient.GetEntry<TestingContentItem>(string.Empty, string.Empty, string.Empty);

        // assert
        _contentfulClient.Received().GetEntry<TestingContentItem>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetEntry_Overload_1()
    {
        // act
        _cpdClient.GetEntry(string.Empty, new QueryBuilder<TestingContentItem>());

        // assert
        _contentfulClient.Received().GetEntry(Arg.Any<string>(), Arg.Any<QueryBuilder<TestingContentItem>>());
    }

    [Test]
    public void Calls_Base_GetEntry_Overload_2()
    {
        // act
        _cpdClient.GetEntry<TestingContentItem>(string.Empty);

        // assert
        _contentfulClient.Received().GetEntry<TestingContentItem>(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetLocales()
    {
        // act
        _cpdClient.GetLocales(string.Empty);

        // assert
        _contentfulClient.Received().GetLocales(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetLocales_Overload()
    {
        // act
        _cpdClient.GetLocales(new CancellationToken());

        // assert
        _contentfulClient.Received().GetLocales(Arg.Any<CancellationToken>());
    }

    [Test]
    public void Calls_Base_GetSpace()
    {
        // act
        _cpdClient.GetSpace(string.Empty);

        // assert
        _contentfulClient.Received().GetSpace(Arg.Any<string>());
    }

    [Test]
    public void Calls_Base_GetSpace_Overload()
    {
        // act
        _cpdClient.GetSpace(new CancellationToken());

        // assert
        _contentfulClient.Received().GetSpace(Arg.Any<CancellationToken>());
    }

    [Test]
    public void Calls_Base_SyncInitial()
    {
        // act
        _cpdClient.SyncInitial();

        // assert
        _contentfulClient.Received().SyncInitial();
    }

    [Test]
    public void Calls_Base_SyncInitialRecursive()
    {
        // act
        _cpdClient.SyncInitialRecursive();

        // assert
        _contentfulClient.Received().SyncInitialRecursive();
    }

    [Test]
    public void Calls_Base_SyncNextResult()
    {
        // act
        _cpdClient.SyncNextResult(string.Empty, default);

        // assert
        _contentfulClient.Received().SyncNextResult(Arg.Any<string>(), default);
    }

    #endregion
}
