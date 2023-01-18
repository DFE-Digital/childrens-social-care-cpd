using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class CookieBanner
    {
        public string CookieBannerHeading { get; set; }
        public Document CookieMessageBody { get; set; }
        public string AcceptCookieButtonText { get; set; }
        public string RejectCookieButtonText { get; set; }
        public Link ViewCookiesLinkText { get; set; }
        public string HideCookieMessageButtonText { get; set; }
        public Document AcceptedCookieMessage { get; set; }
        public Document RejectedCookieMessage { get; set; }
        public string ChangeCookieSettingsMessage { get; set; }
    }
}
