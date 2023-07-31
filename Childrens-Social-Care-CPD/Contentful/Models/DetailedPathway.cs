using Contentful.Core.Models;

namespace Childrens_Social_Care_CPD.Contentful.Models
{
    public class DetailedPathway : IContent
    {
        public string Title { get; set; }
        public string Pathway { get; set; }
        public string Audience { get; set; }
        public string Duration { get; set; }
        public string TimeCommitment { get; set; }
        public string Cost { get; set; }
        public Document DeliveredBy { get; set; }
        public Document WhoItsFor { get; set; }
        public Document WhatYoullLearn { get; set; }
        public Document WhatYoullGet { get; set; }
        public Document HowItsDelivered { get; set; }
        public Document Funding { get; set; }
        public Document NextSteps { get; set; }
    }
}
