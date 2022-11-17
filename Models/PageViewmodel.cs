using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageViewModel : BaseViewModel
    {
        public ContentPageName PageName { get; set; }

        public ContentPageType PageType { get; set; }

        public string PageTitle { get; set; }

        public List<Role> Roles { get; set; }

        public List<Paragraph> Paragraphs { get; set; }
    }
}
