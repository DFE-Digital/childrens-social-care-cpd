using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageViewModel : BaseViewModel
    {
        public ContentfulCollection<Role> Roles { get; set; }

        public ContentfulCollection<Paragraph> Paragraphs { get; set; }
    }
}
