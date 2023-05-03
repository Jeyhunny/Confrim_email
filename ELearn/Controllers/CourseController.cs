using ELearn.Data;
using ELearn.Models;
using ELearn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearn.Controllers
{
    public class CourseController : Controller
    {

        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int count = await _context.Courses.Where(m => !m.SoftDelete).CountAsync();

            ViewBag.Count = count;

            IEnumerable<Course> courses = await _context.Courses.Include(m => m.CourseImages).Include(m => m.Owner).Where(m => m.SoftDelete == false).Take(3).ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Course> courses = await _context.Courses
                .Include(m => m.CourseImages)
                .Include(m => m.Owner)
                .Where(m => m.SoftDelete == false)
                .Skip(skip).Take(3).ToListAsync();


            return PartialView("_CoursesPartial", courses);
        }
    }
}
