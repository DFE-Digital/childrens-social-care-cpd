using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;


namespace Childrens_Social_Care_CPD_Tests
{

    public class ErrorControllerTests
    {
        private Mock<ILogger<ErrorController>> _logger;
        private ErrorController _target;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ErrorController>>();
            _target = new ErrorController(_logger.Object);
        }

        [Test]
        public void ErrorPageReturnsModelOfTypeErrorViewModelTest()
        {
            var actual = _target.Error();
            ViewResult viewResult = (ViewResult)actual;
            Assert.IsInstanceOf<ErrorViewModel>(viewResult.Model);
        }

        [Test]
        public void ErrorPageReturnsReturnsCorrectStatusCodeTest()
        {
            var actual = _target.Error((int)System.Net.HttpStatusCode.NotFound);
            ViewResult viewResult = (ViewResult)actual;
            var model = viewResult.ViewData.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, model.ErrorCode);
        }
    }
}