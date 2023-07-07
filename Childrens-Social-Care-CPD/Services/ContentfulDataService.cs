using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Enums;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;

namespace Childrens_Social_Care_CPD.Services
{
    public class ContentfulDataService: IContentfulDataService
    {
        private readonly IContentfulClient _client;
        public ContentfulDataService(IContentfulClient client)
        {
           _client = client;
          
        }

        public async Task<ContentfulCollection<PageViewModel>> GetViewData<T>(string pageName, string pageType)
        {
            int contentLevel = 10;
            ContentPageType contentPageType;

            if (string.IsNullOrEmpty(pageName) && string.IsNullOrEmpty(pageType))
            {
                pageName = PageNames.HomePage.ToString();
                contentPageType = new ContentPageType { PageType = PageTypes.Master.ToString() };
            }
            else
            {
                contentPageType = new ContentPageType { PageType = pageType };
            }

            var queryBuilder = QueryBuilder<PageViewModel>.New.ContentTypeIs(Constants.SiteConstants.PAGE)
                .FieldEquals("fields.pageName.fields.pageName", pageName)
                .FieldEquals("fields.pageName.sys.contentType.sys.id", Constants.SiteConstants.PAGENAMES)
                .Include(contentLevel);

            var result = await _client.GetEntries<PageViewModel>(queryBuilder);
            result.All(c => { c.PageType = contentPageType; return true; });
           
            foreach (PageViewModel viewModel in result)
            {
                viewModel.Cards = viewModel.Cards.OrderBy(x => x.SortOrder).ToList();
                viewModel.Labels = viewModel.Labels.OrderBy(x => x.SortOrder).ToList();
                viewModel.RichTexts = viewModel.RichTexts.OrderBy(x => x.SortOrder).ToList();
            }

            return result;
        }

        /// <summary>
        /// Method to get Header using Contentful API call
        /// </summary>
        /// <returns></returns>
        public async Task<PageHeader> GetHeaderData()
        {
            var queryBuilder = QueryBuilder<PageHeader>.New.ContentTypeIs(SiteConstants.PAGEHEADER);
            var result = await _client.GetEntries<PageHeader>(queryBuilder);
            var header = result.FirstOrDefault();
            var htmlRenderer = new HtmlRenderer();
            var html = htmlRenderer.ToHtml(header?.PrototypeText).Result;

            PageHeader pageHeader = new PageHeader
            {
                Header = header == null ? string.Empty : header.Header,
                PrototypeTextHtml = html,
                PrototypeHeader = header == null ? string.Empty : header.PrototypeHeader,
                HeaderLinkTitle = header.HeaderLinkTitle
            };
            return pageHeader;
        }

        /// <summary>
        /// To get contents for Cookie banner
        /// </summary>
        /// <returns></returns>
        public async Task<CookieBanner> GetCookieBannerData()
        {
            var cookieBannerQueryBuilder = QueryBuilder<CookieBanner>.New.ContentTypeIs(SiteConstants.COOKIEBANNER);
            var cookieBannerResult = await _client.GetEntries<CookieBanner>(cookieBannerQueryBuilder);
            var cookieBanner = cookieBannerResult.FirstOrDefault();
            return cookieBanner;
        }
    }
}
