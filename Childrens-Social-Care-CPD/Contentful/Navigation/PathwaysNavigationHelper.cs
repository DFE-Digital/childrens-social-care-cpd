using Azure;
using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class PathwaysNavigationHelper : INavigationHelper
{
    // fields
    private NavigationLocation _next;
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
                /* 
                    TODO: this line needs a lot of hardening. It will fall over if the pathways
                    module doesn't have sections, or if the first module section doesn't have pages.

                    If it falls over in circumstances like that, the user ought to see a 
                    fairly anodyne "misconfiguration' error
                */
                var url = page.PathwaysModule
                    .Sections
                    .First<PathwaysModuleSection>()
                    .Pages
                    .First<Content>()
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
