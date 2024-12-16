using System.Collections.Generic;
using System.Threading.Tasks;
using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Contentful.Navigation;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD_Tests.Contentful.Navigation;

public class PathwaysNavigationHelperTests
{

    #region Overview Page
    [Test]
    public void Page_Of_Type_Pathways_Overview_Page_Not_Associated_With_Pathways_Module_Should_Render_With_Default_Next ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public void Page_Of_Type_Pathways_Overview_Page_Associated_With_Pathways_Module_With_No_Contents_Page_Configured_Should_Render_With_Default_Next ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage,
            PathwaysModule = new PathwaysModule()
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public void Page_Of_Type_Pathways_Overview_Page_Associated_With_Pathways_Module_With_Contents_Page_Configured_Should_Have_ContentsPage_Id_As_Next_Url ()
    {
        // setup
        var contentsPageId = "CONTENTS_PAGE_ID";
        var page = new Content ()
        {
            PageType = PageType.PathwaysOverviewPage,
            PathwaysModule = new PathwaysModule()
            {
                ContentsPage = new Content ()
                {
                    Id = contentsPageId
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/" + contentsPageId);
    }

    #endregion

    #region Contents Page

    [Test]
    public void Page_Of_Type_Pathways_Contents_Page_Should_Have_First_Page_Of_First_Section_Id_As_Next_Url ()
    {
        // setup
        var trainingPageId = "TRAINING_PAGE_ID";
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = trainingPageId
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/" + trainingPageId);
    }

    [Test]
    public void Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_First_Section_Has_No_Pages ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection ()
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public void Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_Pathway_Has_No_Sections ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage,
            PathwaysModule = new PathwaysModule ()
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    [Test]
    public void Page_Of_Type_Pathways_Contents_Page_Should_Have_Default_Next_Url_If_Page_Has_No_Pathway_Modules ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysContentsPage
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Url.Should().Be("/");
    }

    #endregion

    #region Training Content Pages

    [Test]
    public void First_Page_In_First_Section_Of_Module_Should_Have_Second_Page_Of_First_Section_As_Next_Url_And_Module_Contents_Page_As_Previous_Url ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "section 1 page 1",
            PathwaysModule = new PathwaysModule()
            {
                ContentsPage = new Content {
                    Id = "contents page"
                },
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 1 page 1"
                            },
                            new Content()
                            {
                                Id = "section 1 page 2"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Next");
        sut.Next.Url.Should().Be("/section 1 page 2");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/contents page");
    }

    [Test]
    public void Middle_Page_In_First_Section_Of_Module_Should_Have_Next_Page_Of_Same_Section_As_Next_Url_And_Previous_Page_Of_Same_Section_As_Previous_Url ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "section 1 page 2",
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 1 page 1"
                            },
                            new Content()
                            {
                                Id = "section 1 page 2"
                            },
                            new Content()
                            {
                                Id = "section 1 page 3"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Next");
        sut.Next.Url.Should().Be("/section 1 page 3");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/section 1 page 1");
    }

    [Test]
    public void Last_Page_In_Not_Last_Section_Of_Module_Should_Have_First_Page_Of_Next_Section_As_Next_Url_And_Previous_Page_Of_Same_Section_As_Previous_Url ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "section 1 page 3",
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 1 page 1"
                            },
                            new Content()
                            {
                                Id = "section 1 page 2"
                            },
                            new Content()
                            {
                                Id = "section 1 page 3"
                            }
                        }
                    },
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 2 page 1"
                            },
                            new Content()
                            {
                                Id = "section 2 page 2"
                            },
                            new Content()
                            {
                                Id = "section 2 page 3"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Next");
        sut.Next.Url.Should().Be("/section 2 page 1");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/section 1 page 2");
    }

    [Test]
    public void First_Page_In_Not_First_Section_Of_Module_Should_Have_Next_Page_Of_Same_Section_As_Next_Url_And_Last_Page_Of_Previous_Section_As_Previous_Url ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "section 2 page 1",
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 1 page 1"
                            },
                            new Content()
                            {
                                Id = "section 1 page 2"
                            },
                            new Content()
                            {
                                Id = "section 1 page 3"
                            }
                        }
                    },
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 2 page 1"
                            },
                            new Content()
                            {
                                Id = "section 2 page 2"
                            },
                            new Content()
                            {
                                Id = "section 2 page 3"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Next");
        sut.Next.Url.Should().Be("/section 2 page 2");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/section 1 page 3");
    }

    [Test]
    public void Last_Page_In_Last_Section_Of_Module_Should_Have_All_Pathways_Page_Next_Url_And_Previous_Page_Of_Same_Section_As_Previous_Url ()
    {
        // setup
        var page = new Content ()
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "section 2 page 3",
            PathwaysModule = new PathwaysModule()
            {
                Sections = new List<PathwaysModuleSection>()
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 1 page 1"
                            },
                            new Content()
                            {
                                Id = "section 1 page 2"
                            },
                            new Content()
                            {
                                Id = "section 1 page 3"
                            }
                        }
                    },
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>() 
                        {
                            new Content()
                            {
                                Id = "section 2 page 1"
                            },
                            new Content()
                            {
                                Id = "section 2 page 2"
                            },
                            new Content()
                            {
                                Id = "section 2 page 3"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Go back to all pathways");
        sut.Next.Url.Should().Be("/all-pathways");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/section 2 page 2");
    }

    [Test]
    public void Pathways_Training_Content_Page_Should_Render_Without_Next_And_Previous_If_Pathway_Has_No_Sections ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent,
            PathwaysModule = new PathwaysModule ()
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Should().Be(null);
        sut.Previous.Should().Be(null);
    }

    [Test]
    public void Pathways_Training_Content_Page_Should_Render_Without_Next_And_Previous_If_Page_Has_No_Pathway ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Should().Be(null);
        sut.Previous.Should().Be(null);
    }

    [Test]
    public void Pathways_Training_Content_Page_Should_Render_Without_Next_And_Previous_If_Page_Not_Listed_In_Pathway_Section ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "PAGE_ID",
            PathwaysModule = new PathwaysModule
            {
                Sections = new List<PathwaysModuleSection>
                {
                    new PathwaysModuleSection
                    {
                        Pages = new List<Content>
                        {
                            new Content { Id = "Page 1" },
                            new Content { Id = "Page 2" },
                            new Content { Id = "Page 3" }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Should().Be(null);
        sut.Previous.Should().Be(null);
    }

    [Test]
    public void Pathways_Training_Content_Page_Should_Render_Without_Next_And_Previous_If_Module_Sections_Have_No_Pages ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "PAGE_ID",
            PathwaysModule = new PathwaysModule
            {
                Sections = new List<PathwaysModuleSection>
                {
                    new PathwaysModuleSection { },
                    new PathwaysModuleSection { },
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Should().Be(null);
        sut.Previous.Should().Be(null);
    }

    [Test]
    public void Pathways_Training_Content_Page_Which_Is_Only_Page_In_Only_Section_Should_Have_Contents_Page_As_Previous_And_All_Pathways_As_Next ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "PAGE_ID",
            PathwaysModule = new PathwaysModule
            {
                ContentsPage = new Content {
                    Id = "CONTENTS_PAGE_ID"
                },
                Sections = new List<PathwaysModuleSection>
                {
                    new PathwaysModuleSection {
                        Pages = new List<Content>
                        {
                            new Content
                            {
                                Id = "PAGE_ID"
                            }
                        }
                    }
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.Next.Name.Should().Be("Go back to all pathways");
        sut.Next.Url.Should().Be("/all-pathways");
        sut.Previous.Name.Should().Be("Previous");
        sut.Previous.Url.Should().Be("/CONTENTS_PAGE_ID");
    }

    [Test]
    public void Pathways_Training_Content_Page_Should_Render_With_Current_Location_Info ()
    {
        // setup
        var page = new Content
        {
            PageType = PageType.PathwaysTrainingContent,
            Id = "PAGE_ID",
            PathwaysModule = new PathwaysModule
            {
                ContentsPage = new Content {
                    Id = "CONTENTS_PAGE_ID"
                },
                Sections = new List<PathwaysModuleSection>
                {
                    new PathwaysModuleSection (),
                    new PathwaysModuleSection {
                        Name = "SECTION_NAME",
                        Pages = new List<Content>
                        {
                            new Content
                            {
                                Id = "PAGE_ID"
                            }
                        }
                    },
                    new PathwaysModuleSection ()
                }
            }
        };

        // act
        var sut = new PathwaysNavigationHelper(page);

        // assert
        sut.CurrentLocation.SectionName.Should().Be("SECTION_NAME");
        sut.CurrentLocation.SectionNumber.Should().Be(2);
        sut.CurrentLocation.TotalSections.Should().Be(3);
    }

    #endregion
}