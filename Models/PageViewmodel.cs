using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageViewModel : BaseViewModel
    {
        public ContentPageName PageName { get; set; }

        public string PageTitle { get; set; }
        
        public string PageHeading { get; set; }

        public string PageSubHeading { get; set; }

        public ContentPageType PageType { get; set; }

        public List<Card> Cards { get; set; }

        public List<Paragraph> Paragraphs { get; set; }

        public List<Link> Links { get; set; }

        public List<Label> Labels { get; set; }

        public List<RichText> RichTexts { get; set; }
    }
}
