namespace Childrens_Social_Care_CPD.Models
{
    public class Role
    {
        public string CareerStage { get; set; }
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }
        public RedirectPageName RoleDetailsPageName { get; set; }
        public string RoleDescription { get; set; }
        public string SalaryRange { get; set; }
        public string SalaryRangeLabel { get; set; }
        public int SortOrder { get; set; }
    }
}
