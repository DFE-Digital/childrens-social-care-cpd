using Azure;
using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class PathwaysNavigationHelper : INavigationHelper
{
    // fields
    private readonly NavigationLocation _next;
    /*
    * restore this when implementing main training content
    private NavigationLocation _previous;
    */

    // properties
    public NavigationLocation Next
    {
        get
        {
            return _next;
        }
    }
    /*
    * restore this when implementing main training content
    public NavigationLocation Previous
    {
        get
        {
            return _previous;
        }
    }
    */

    public PathwaysNavigationHelper (Content page)
    {
        switch (page.PageType)
        {
            case PageType.PathwaysOverviewPage:
                this._next = new NavigationLocation
                {
                    Url = "/" + page.PathwaysModule?.ContentsPage?.Id
                };
            break;

            case PageType.PathwaysContentsPage:
                var url = page.PathwaysModule?
                    .Sections?[0]
                    .Pages?[0]
                    .Id;

                this._next = new NavigationLocation
                {
                    Url = "/" + url
                };
            break;

            case PageType.PathwaysTrainingContent:
                /*
                next page depends on position of current page in sections
                all but final page just need the next page in the sequence

                previous page depends on position of current page in sections
                */
            break;
        }
    }
}
