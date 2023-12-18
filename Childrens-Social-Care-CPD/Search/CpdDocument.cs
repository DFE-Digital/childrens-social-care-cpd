namespace Childrens_Social_Care_CPD.Search;

public partial class CpdDocument
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public string Body { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; }
}