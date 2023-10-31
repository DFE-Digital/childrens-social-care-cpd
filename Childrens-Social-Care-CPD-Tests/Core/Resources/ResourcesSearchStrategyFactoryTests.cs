using Castle.Core.Logging;
using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Childrens_Social_Care_CPD_Tests.Core.Resources;

public class ResourcesSearchStrategyFactoryTests
{
    [TestCase(true, typeof(ResourcesDynamicTagsSearchStategy))]
    [TestCase(false, typeof(ResourcesFixedTagsSearchStrategy))]
    public void Creates_Correct_Strategy(bool isFeatureOn, Type type)
    {
        // arrange
        var featuresConfig = Substitute.For<IFeaturesConfig>();
        featuresConfig.IsEnabled(Features.ResourcesUseDynamicTags).Returns(isFeatureOn);
        var sut = new ResourcesSearchStrategyFactory(featuresConfig, Substitute.For<IResourcesRepository>(), Substitute.For<ILogger<ResourcesFixedTagsSearchStrategy>>());

        // act
        var actual = sut.Create();

        // assert
        actual.Should().BeOfType(type);
    }
}
