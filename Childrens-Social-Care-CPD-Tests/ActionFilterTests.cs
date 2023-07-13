using System.Collections.Generic;
using System.Linq;
using Childrens_Social_Care_CPD.ActionFilters;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ActionDescriptor = Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor;
using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace Childrens_Social_Care_CPD_Tests
{

    public class ActionFilterTests
    {
        private Mock<IContentfulDataService> _contentfulDataService;
        private CPDActionFilter _target;
        private CPDController _controller;
        private Mock<ILogger<CPDActionFilter>> _logger;

        [SetUp]
        public void Setup()
        {
            _contentfulDataService = new Mock<IContentfulDataService>(MockBehavior.Loose);
            _logger = new Mock<ILogger<CPDActionFilter>>();
            _target = new CPDActionFilter(_logger.Object, _contentfulDataService.Object);
            _controller =  new CPDController(null,null);
        }

        private ActionExecutingContext SetActionExecutingContext()
        {
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext();
            var context = new Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                _controller);
            return context;
        }
    }
}