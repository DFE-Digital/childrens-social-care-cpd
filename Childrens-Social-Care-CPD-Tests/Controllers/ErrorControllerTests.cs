using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;


namespace Childrens_Social_Care_CPD_Tests.Controllers
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
        public void ErrorPageReturnsStatusCodeResult()
        {
            var actual = _target.Error() as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be(500);
        }
    }
}