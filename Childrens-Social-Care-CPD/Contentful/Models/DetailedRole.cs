using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models;

public class DetailedRole : IContent
{
    public string Title { get; set; }
    public string SalaryRange { get; set; }
    public string Summary { get; set; }
    public string RoleListSummary { get; set; }
    public string OtherNames { get; set; }
    public Document WhatYoullDo { get; set; }
    public Document SkillsAndKnowledge { get; set; }
    public Document HowToBecomeOne { get; set; }
    public Document CareerPathsAndProgression { get; set; }
    public Document CurrentOpportunities { get; set; }
}
