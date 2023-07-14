using System.Collections.Generic;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;


namespace Childrens_Social_Care_CPD_Tests.Controllers
{
    public class CPDControllerTests
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        private Mock<ILogger<CPDController>> _logger;
        private CPDController _target;

        [SetUp]
        public void Setup()
        {
            _contentfulDataService = new Mock<IContentfulDataService>(MockBehavior.Strict);
            _contentfulDataService.Setup(c => c.GetViewData<PageViewModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(MockData.Pages);
            _logger = new Mock<ILogger<CPDController>>();
            _target = new CPDController(_logger.Object, _contentfulDataService.Object);
        }

        [Test]
        public void LandingPageReturnsModelOfTypePageViewModelTest()
        {
            var actual = _target.LandingPage(null, null, null, null);
            ViewResult viewResult = (ViewResult)actual.Result;
            Assert.IsInstanceOf<ContentfulCollection<PageViewModel>>(viewResult.Model);
        }
    }
}