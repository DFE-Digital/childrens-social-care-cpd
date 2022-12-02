namespace Childrens_Social_Care_CPD.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorMessage { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string ErrorPath { get; set; }
        public string InnerException { get; set; }
    }
}