namespace ELearn.Models
{
    public class Owner: BaseEntity
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
