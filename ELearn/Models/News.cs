namespace ELearn.Models
{
    public class News: BaseEntity
    {
        public string Image { get; set; }
        public DateTime Time  { get; set; }
        public string Header { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

    }
}
