namespace Childrens_Social_Care_CPD.Models
{
    public class CareerStage
    {
        public string CareerStageName { get; set; }
        public List<Role> Roles { get; set; }
        public int SortOrder { get; set; }
    }
}
