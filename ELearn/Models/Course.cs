using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace ELearn.Models
{
    public class Course:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sales { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Price { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<CourseImage> CourseImages { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }




    }
}
