using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;


namespace Childrens_Social_Care_CPD_Tests
{

    public class AppInfoControllerTests
    {
        private AppInfoController _target;

        [SetUp]
        public void Setup()
        {
            _target = new AppInfoController();
        }

        [Test]
        public void AppInfoReturnsApplicationInfoTest()
        {
            var actual = _target.AppInfo();
            var c = actual.Value;
            Assert.IsInstanceOf<JsonResult>(actual);
            Assert.IsNotNull(((ApplicationInfo)actual.Value).Environment);
        }
    }
}