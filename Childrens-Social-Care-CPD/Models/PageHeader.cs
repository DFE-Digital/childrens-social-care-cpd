using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class PageHeader
    {
        public string Header { get; set; } = string.Empty;
        public Document PrototypeText { get; set; }
        public string PrototypeHeader { get; set; } = string.Empty;
        public string PrototypeTextHtml { get; set; } = string.Empty;
    }
}
