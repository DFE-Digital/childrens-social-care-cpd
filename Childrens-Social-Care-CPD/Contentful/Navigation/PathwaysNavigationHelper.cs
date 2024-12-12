using Azure;
using Childrens_Social_Care_CPD.Contentful.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class PathwaysNavigationHelper : INavigationHelper
{
    // fields
    private NavigationLocation _next;
    private NavigationLocation _previous;

    // properties
    public NavigationLocation Next
    {
        get
        {
            return _next;
        }
    }
    public NavigationLocation Previous
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
                int sectionCounter = 0;
                bool stop = false;

                page.PathwaysModule.Sections.ForEach(section => {
                    if (stop) return;

                    var pageCounter = 0;

                    section.Pages.ForEach(sectionPage => {
                        if (stop) return;

                        if (sectionPage.Id == page.Id) {
                            stop = true;
                            if (pageCounter < section.Pages.Count()-1) {
                                // not the last page in the section, make the next link navigate to the next page in the section
                                this._next = new NavigationLocation {
                                    Name = "Next",
                                    Url = "/" + section.Pages[pageCounter + 1].Id
                                };
                            }
                            else 
                            {
                                if (sectionCounter < page.PathwaysModule.Sections.Count()-1) {
                                    // last page in section, but not last section in the module, next navigates to first page in next section
                                    this._next = new NavigationLocation {
                                        Name = "Next",
                                        Url = "/" + page.PathwaysModule.Sections[sectionCounter + 1].Pages[0].Id
                                    };
                                }
                                else{
                                    // last page in last module, next navigates to 'all pathways page'
                                    this._next = new NavigationLocation {
                                        Url = "/all-pathways",
                                        Name = "Go back to all pathways"
                                    };
                                }
                            }

                            if (pageCounter > 0) {
                                // not the first page in the section, so previous navigates to previous page
                                this._previous = new NavigationLocation
                                {
                                    Name = "Previous",
                                    Url = "/" + section.Pages[pageCounter - 1].Id
                                };
                            }
                            else 
                            {
                                if (sectionCounter > 0) {
                                    // first page in section, but not first section, previous should navigate back to last page in previous section
                                    this._previous = new NavigationLocation
                                    {
                                        Name = "Previous",
                                        Url = "/" + page
                                            .PathwaysModule
                                            .Sections[sectionCounter - 1]
                                            .Pages[page.PathwaysModule.Sections[sectionCounter - 1].Pages.Count()-1]
                                            .Id
                                    };
                                }
                                else
                                {
                                    // first page in first section, previous should navigate back to contents page
                                    this._previous = new NavigationLocation
                                    {
                                        Name = "Previous",
                                        Url = "/" + page.PathwaysModule.ContentsPage.Id
                                    };
                                }
                            }
                        }
                        pageCounter++;
                    });
                    sectionCounter++;
                });

            break;
        }
    }
}
