using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class CookiesAndAnalyticsConsentModel
    {
        public Content Content { get; set; }
        public AnalyticsConsentState ConsentState { get; set; }
        public string SourceUrl { get; set; }
        public bool PreferencesSet { get; set; }
    }
}
