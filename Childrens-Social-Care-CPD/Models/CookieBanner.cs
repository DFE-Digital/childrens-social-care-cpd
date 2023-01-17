namespace Childrens_Social_Care_CPD.Models
{
    public class CookieBanner
    {
        public string BannerHeader { get; set; }
        public List<string> BannerDescription { get; set; }
        public string AcceptButtonText { get; set; }
        public string RejectButtonText { get; set; }
        public string ViewCookiesLinkText { get; set; }
        public string HideCookieMessageButtonText { get; set; }
        public string AcceptedCookieMessage { get; set; }
        public string RejectedCookieMessage { get; set; }
    }
}
