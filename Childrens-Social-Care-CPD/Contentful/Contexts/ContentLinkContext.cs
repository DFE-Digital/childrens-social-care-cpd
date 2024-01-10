﻿using Childrens_Social_Care_CPD.Contentful.Renderers;

namespace Childrens_Social_Care_CPD.Contentful.Contexts
{
    public class ContentLinkContext : IContentLinkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContentLinkContext(IHttpContextAccessor httpContextAccessor) 
        { 
            _httpContextAccessor = httpContextAccessor;
        }
        public string Path => _httpContextAccessor.HttpContext?.Request.Path;
    }
}
