using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core.Errors;
using Contentful.Core.Models;
using Contentful.Core.Search;
using System.Threading;
using Newtonsoft.Json;
using Contentful.Core.Configuration;
using Contentful.Core.Models.Management;
using Childrens_Social_Care_CPD.Models;

namespace Childrens_Social_Care_CPD.Interfaces
{
    public interface IContentfulDataService
    {

        /// <summary>
        /// Method to get contentful content using API call
        /// </summary>
        /// <param name="pageName">Contentful page name</param>
        /// <param name="pageType">Contentful page type</param>
        /// <returns></returns>
        Task<ContentfulCollection<PageViewModel>> GetViewData<T>(string pageName, string pageType);

        /// <summary>
        /// To get contents for Cookie banner
        /// </summary>
        /// <returns></returns>
        Task<CookieBanner> GetCookieBannerData();
    }
}
