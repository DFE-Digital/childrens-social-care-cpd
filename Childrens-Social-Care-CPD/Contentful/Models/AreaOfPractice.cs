using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class AreaOfPractice : IContent
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public string AreaOfPracticeListSummary { get; set; }
    public string OtherNames { get; set; }
    public Document WhatYoullDo { get; set; }
    public Document SkillsAndKnowledge { get; set; }
    public Document WhoYouWillWorkWith { get; set; }
    public Document HowYouWillWork { get; set; }
    public Document CurrentOpportunities { get; set; }
}
