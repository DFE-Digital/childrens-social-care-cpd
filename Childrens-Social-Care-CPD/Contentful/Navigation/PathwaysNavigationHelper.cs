using Childrens_Social_Care_CPD.Contentful.Models;

namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public class PathwaysNavigationHelper : INavigationHelper
{
    // fields
    private NavigationLocation _next;
    private NavigationLocation _previous;
    private readonly NavigationLocation _availablePathwaysPage = new NavigationLocation
    {
        Url = "/pathways-social-work-leadership-modules/available-pathways"
    };
    private LocationInfo _currentLocation;


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

    public NavigationLocation AvailablePathwaysPage
    {
        get
        {
            return _availablePathwaysPage;
        }
    }

    public LocationInfo CurrentLocation
    {
        get
        {
            return _currentLocation;
        }
    }

    public PathwaysNavigationHelper(Content page)
    {
        switch (page.PageType)
        {
            case PageType.PathwaysOverviewPage:
                string url = "/";
                if (page.PathwaysModule?.ContentsPage != null)
                {
                    url += page.PathwaysModule.ContentsPage.Id;
                }
                else
                {
                    url += GetFirstSectionFirstPageId(page);
                }

                this._next = new NavigationLocation
                {
                    Url = url,
                    Name = page.PathwaysModule?.Type == PathwaysModuleType.IntroductoryModule
                        ? "Start module"
                        : "Start pathway"
                };
                break;

            case PageType.PathwaysContentsPage:

                this._next = new NavigationLocation
                {
                    Url = "/" + GetFirstSectionFirstPageId(page)
                };

                this._previous = new NavigationLocation
                {
                    Url = "/" + page.PathwaysModule?.OverviewPage?.Id,
                    Name = "Back to " + page.PathwaysModule?.OverviewPage?.BreadcrumbText
                };
                break;

            case PageType.PathwaysTrainingContent:
                int sectionCounter = 0;

                page.PathwaysModule?.Sections?.ForEach(section =>
                {

                    var pageCounter = 0;

                    section.Pages?.ForEach(sectionPage =>
                    {

                        if (sectionPage.Id == page.Id)
                        {
                            SetTrainingPageNextNavigation(pageCounter, sectionCounter, page, section);
                            SetTrainingPagePreviousNavigation(pageCounter, sectionCounter, page, section);
                            SetTrainingPageCurrentLocation(sectionCounter, page, section, pageCounter);
                        }

                        pageCounter++;

                    });
                    sectionCounter++;
                });

                break;

            case PageType.AllPathwaysOverviewPage:
                this._next = AvailablePathwaysPage;
                break;
        }
    }

    private void SetTrainingPageNextNavigation(int pageCounter, int sectionCounter, Content page, PathwaysModuleSection section)
    {
        if (pageCounter < section.Pages.Count - 1)
        {
            // not the last page in the section, make the next link navigate to the next page in the section
            this._next = new NavigationLocation
            {
                Name = "Next",
                Url = "/" + section.Pages[pageCounter + 1].Id
            };
        }
        else
        {
            if (sectionCounter < page.PathwaysModule.Sections.Count - 1)
            {
                // last page in section, but not last section in the module, next navigates to first page in next section
                this._next = new NavigationLocation
                {
                    Name = "Next",
                    Url = "/" + page.PathwaysModule.Sections[sectionCounter + 1].Pages?[0].Id
                };
            }
            else
            {
                // last page in last module, next navigates to 'Available pathways page'
                this._next = new NavigationLocation
                {
                    Url = AvailablePathwaysPage.Url,
                    Name = "Go back to Available pathways"
                };
            }
        }
    }

    private void SetTrainingPagePreviousNavigation(int pageCounter, int sectionCounter, Content page, PathwaysModuleSection section)
    {
        if (pageCounter > 0)
        {
            // not the first page in the section, so previous navigates to previous page
            this._previous = new NavigationLocation
            {
                Name = "Previous",
                Url = "/" + section.Pages[pageCounter - 1].Id
            };
        }
        else
        {
            if (sectionCounter > 0)
            {
                // first page in section, but not first section, previous should navigate back to last page in previous section
                this._previous = new NavigationLocation
                {
                    Name = "Previous",
                    Url = "/" + page
                        .PathwaysModule
                        .Sections[sectionCounter - 1]
                        .Pages?[page.PathwaysModule.Sections[sectionCounter - 1].Pages.Count - 1]
                        .Id
                };
            }
            else
            {
                // first page in first section, previous should navigate back to contents page
                // or overview page if there is no contents page
                string url = "/";
                if (page.PathwaysModule?.ContentsPage != null)
                {
                    url += page.PathwaysModule.ContentsPage.Id;
                }
                else
                {
                    url += page.PathwaysModule.OverviewPage?.Id;
                }

                this._previous = new NavigationLocation
                {
                    Name = "Previous",
                    Url = url
                };
            }
        }
    }

    private void SetTrainingPageCurrentLocation(int currentSectionIdx, Content page, PathwaysModuleSection section, int currentPageIdx)
    {   
        bool isFirstPageOfFirstSection = currentPageIdx == 0 && currentSectionIdx == 0;
        bool isLastSection = currentSectionIdx + 1 == page.PathwaysModule.Sections.Count;
        this._currentLocation = new LocationInfo
        {
            SectionName = section.Name,
            SectionNumber = currentSectionIdx + 1,
            TotalSections = page.PathwaysModule.Sections.Count,
            PageNumber = currentPageIdx + 1,
            IsFirstPageOfFirstSection = isFirstPageOfFirstSection,
            IsLastSection = isLastSection
        };

    }

    private string GetFirstSectionFirstPageId (Content page)
    {
        return page
            .PathwaysModule?
            .Sections?[0]
            .Pages?[0]
            .Id;
    }
}
