namespace ELearn.Models
{
    public class CourseImage:BaseEntity
    {
        public string? Image { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public bool IsMain { get; set; } = false;

    }
}
