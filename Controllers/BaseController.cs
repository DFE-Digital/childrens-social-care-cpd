using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Childrens_Social_Care_CPD.Controllers
{
    public class BaseController : Controller
    {
        private IContentfulClient _client;

        public BaseController(IContentfulClient client)
        {
            this._client = client;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            PageHeader pageHeader = GetHeader();

            ViewBag.PageHeader = pageHeader;

            PageFooter pageFooter = GetFooter();

            ViewBag.PageFooter = pageFooter;
        }
      
        private PageFooter GetFooter()
        {
            var footerQueryBuilder = QueryBuilder<PageFooter>.New.ContentTypeIs(ContentTypes.PAGEFOOTER);
            var footerResult = _client.GetEntries<PageFooter>(footerQueryBuilder).Result;
            var footer = footerResult.FirstOrDefault();


            PageFooter pageFooter = new PageFooter
            {
                PrivacyLinkText = footer.PrivacyLinkText,

                PrivacyLinkURL = footer.PrivacyLinkURL,

                AccessibilityLinkText = footer.AccessibilityLinkText,

                AccessibilityLinkURL = footer.AccessibilityLinkURL,

                CookiesLinkText = footer.CookiesLinkText,

                CookiesLinkURL = footer.CookiesLinkURL,

                TermsAndConditionsLinkText = footer.TermsAndConditionsLinkText,

                TermsAndConditionsLinkURL = footer.TermsAndConditionsLinkURL,

                GOVPrototypeLinkText = footer.GOVPrototypeLinkText,

                GOVPrototypeLinkURL = footer.GOVPrototypeLinkURL,

                ClearDataLinkText = footer.ClearDataLinkText,

                ClearDataLinkURL = footer.ClearDataLinkURL,

                CopyrightLinkText = footer.CopyrightLinkText,

                CopyrightLinkURL = footer.CopyrightLinkURL
            };
            return pageFooter;
        }

        private PageHeader GetHeader()
        {
            var queryBuilder = QueryBuilder<PageHeader>.New.ContentTypeIs(ContentTypes.PAGEHEADER);
            var result = _client.GetEntries<PageHeader>(queryBuilder).Result;
            var header = result.FirstOrDefault();
            var htmlRenderer = new HtmlRenderer();
            var html = htmlRenderer.ToHtml(header.PrototypeText).Result;

            PageHeader pageHeader = new PageHeader
            {
                Header = header.Header,
                PrototypeTextHtml = html,
                PrototypeHeader = header.PrototypeHeader
            };
            return pageHeader;
        }
    }
}
