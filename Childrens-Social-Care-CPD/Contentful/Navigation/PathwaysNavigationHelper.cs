using Azure;
using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class PathwaysNavigationHelper : INavigationHelper
{
    // fields
    private NavigationLocation _next;
    private NavigationLocation _previous;

    // properties
    NavigationLocation INavigationHelper.Next 
    {
        get
        {
            return _next;
        }
    }
    NavigationLocation INavigationHelper.Previous 
    {
        get
        {
            return _previous;
        }
    }

    public PathwaysNavigationHelper (Content page)
    {
        switch (page.PageType)
        {
            case PageType.PathwaysOverviewPage:
                this._next = new NavigationLocation
                {
                    Name = "Start Pathway >",
                    Url = "/" + page.PathwaysModule.OverviewPage.Id
                };
            break;

            case PageType.PathwaysContentsPage:
                // next page is section 1 page 1
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