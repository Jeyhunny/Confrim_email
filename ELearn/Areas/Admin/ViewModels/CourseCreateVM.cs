using ELearn.Models;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Areas.Admin.ViewModels
{
    public class CourseCreateVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public int Sales { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public DateTime PublishDate { get; set; } = DateTime.Now;
        [Required]
        public List<IFormFile> Photos{ get; set; }
        public int OwnerId { get; set; }

    }
}
