using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageFooter
    {
        public List<Link> FooterLinks { get; set; }

        public Document LicenceDescription { get; set; }

        public Link CopyrightLink { get; set; } = new Link();

        public string LicenceDescriptionText { get; set; }
    }
}
