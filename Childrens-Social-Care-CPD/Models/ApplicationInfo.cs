using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Models
{
    public class ApplicationInfo
    {
        public string Environment { get; set; }
        public string ContentfulEnvironment { get; set; }
        public string GitShortHash { get; set; }
    }
}
