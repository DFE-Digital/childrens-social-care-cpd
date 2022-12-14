using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Models
{
    public class CardGroup
    {
        public string GroupName { get; set; }
        public string CardGroupType { get; set; }
        public Document GroupContents { get; set; }
        public int SortOrder { get; set; }
    }
}
