
namespace Childrens_Social_Care_CPD.Models
{
    public class PageViewModel : BaseViewModel
    {
        public ContentPageName PageName { get; set; }

        public string PageTitle { get; set; } = string.Empty;
        
        public string PageHeading { get; set; } = string.Empty;

        public string PageSubHeading { get; set; } = string.Empty;

        public ContentPageType PageType { get; set; }

        public List<Card> Cards { get; set; } = new();

        public List<Paragraph> Paragraphs { get; set; } = new();

        public List<Link> Links { get; set; } = new();

        public List<Label> Labels { get; set; } = new();

        public List<RichText> RichTexts { get; set; } = new();
    }
}
