using Childrens_Social_Care_CPD.Enums;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class RichText
    {
        public string Heading { get; set; }

        public string SubHeading { get; set; }

        public Document RichTextContents { get; set; }

        public int SortOrder { get; set; }

        public RichTextRenderType RenderType { get; set; }
    }
}
