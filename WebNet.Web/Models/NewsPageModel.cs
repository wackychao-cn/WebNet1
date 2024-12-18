using WebNet.Model;

namespace WebNet.Web.Models
{
    public class NewsPageModel
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Caname { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
