using ELearn.Models;

namespace ELearn.ViewModel
{
    public class HomeVM
    {
       public  IEnumerable<Slider> Sliders { get; set; }
       public IEnumerable<Course> Courses { get; set; }
       public Course LastCourse { get; set; }
       public IEnumerable<Event> Events { get; set; }
       public IEnumerable<News> News { get; set; }
       public IEnumerable<Publisher> Publishers { get; set; }


    }
}
