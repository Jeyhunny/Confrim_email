using ELearn.Data;
using ELearn.Models;
using ELearn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace ELearn.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m => m.SoftDelete == false).ToListAsync();

            IEnumerable<Course> courses = await _context.Courses.Include(m => m.CourseImages).Include(m => m.Owner).ToListAsync();

            Course lastCourse = await _context.Courses.Include(m => m.CourseImages).Include(m => m.Owner).Where(m => m.SoftDelete == false).OrderByDescending(m => m.Id).FirstOrDefaultAsync();

            IEnumerable<Event> events = await _context.Events.Where(m => m.SoftDelete == false).ToListAsync();

            IEnumerable<News> news = await _context.News.Include(m => m.Publisher).Where(m => m.SoftDelete == false).ToListAsync();



            HomeVM model = new()
            {
                Sliders = sliders,
                Courses = courses,
                LastCourse = lastCourse,
                Events = events,
                News = news
            };

            return View(model);
        }


    }
}