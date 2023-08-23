namespace Childrens_Social_Care_CPD.Contentful.Models;

public class RelatedContent : List<ContentLink>
{
    public RelatedContent()
    {
    }

    public RelatedContent(IEnumerable<ContentLink> collection) : base(collection)
    {
    }

    public RelatedContent(int capacity) : base(capacity)
    {
    }
}
