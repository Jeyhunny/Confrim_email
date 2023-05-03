namespace ELearn.Models
{
    public class Event: BaseEntity
    {
        public string? Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public string? Location { get; set; }

    }
}
