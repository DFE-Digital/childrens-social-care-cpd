using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Models;

public record ContextModel(string Id, string Title, string PageName, string Category, bool UseContainers, bool PreferenceSet, bool HideConsent = false, ContentLink BackLink = null)
{
    public Stack<string> ContentStack { get; } = new Stack<string>();
}
