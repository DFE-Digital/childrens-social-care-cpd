using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageFooter
    {
        public List<Link> FooterLinks { get; set; } = new();

        public Document LicenceDescription { get; set; }

        public Link CopyrightLink { get; set; } = new();

        public string LicenceDescriptionText { get; set; }  
    }
}
