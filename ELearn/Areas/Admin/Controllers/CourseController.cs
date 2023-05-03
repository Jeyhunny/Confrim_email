using ELearn.Areas.Admin.ViewModels;
using ELearn.Data;
using ELearn.Models;
using ELearn.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ELearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env; 
        }

        public async  Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _context.Courses.Include(m=>m.CourseImages).Include(m=>m.Owner).Where(m => m.SoftDelete == false).OrderByDescending(m=>m.Id).ToListAsync();
            return View(courses);
        }

        [HttpGet]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest(); 
            Course course = await _context.Courses.Include(m => m.CourseImages).Include(m => m.Owner).Where(m => m.SoftDelete == false).FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.owners = await GetOwnersAsync();

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateVM model)
        {
            try
            {
                ViewBag.owners = await GetOwnersAsync(); 

                if (!ModelState.IsValid) 
                {
                    return View(model);
                }
                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }
       
                }


                List<CourseImage> courseImages = new(); 
                foreach (var photo in model.Photos)  
                {

                    string fileName = Guid.NewGuid().ToString() + " " + photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);
                    await FileHelper.SaveFileAsync(newPath, photo); 

                    CourseImage newCourseImage = new()
                    {
                        Image = fileName  
                    };
                    courseImages.Add(newCourseImage);  

                }

                courseImages.FirstOrDefault().IsMain = true;  
                decimal convertedPrice = decimal.Parse(model.Price); 


                Course newCourse = new()  
                {
                    Name = model.Name,  
                    Price = convertedPrice,  
                    Sales = model.Sales,
                    Description = model.Description,  
                    OwnerId = model.OwnerId, 
                    PublishDate= model.PublishDate,
                    CourseImages = courseImages,   
                };

                await _context.CourseImages.AddRangeAsync(courseImages); 

                await _context.Courses.AddAsync(newCourse);  
                
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }

        }




        private async Task<SelectList> GetOwnersAsync()
        {
            IEnumerable<Owner> owners = await _context.Owners.Where(m => m.SoftDelete == false).ToListAsync();

            return new SelectList(owners, "Id", "Name");

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Course dbCourse = await _context.Courses.Include(m=>m.Owner).Include(m=>m.CourseImages).Where(m=>!m.SoftDelete).FirstOrDefaultAsync(m=>m.Id == id);
            if (dbCourse == null) return NotFound();

            return View(dbCourse);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            try
            {

                Course dbCourse = await _context.Courses.Include(m => m.CourseImages).Where(m =>m.Id == id).FirstOrDefaultAsync();

                foreach (var item in dbCourse.CourseImages)  
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "img", item.Image);   

                    FileHelper.DeleteFile(path);
                }

                _context.Courses.Remove(dbCourse);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }




        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            if (id == null) return BadRequest();

            bool result = false;

            CourseImage courseImage = await _context.CourseImages.Where(m => m.Id == id).FirstOrDefaultAsync();


            if (courseImage == null) return NotFound();

            var data = await _context.Courses.Include(m => m.CourseImages).FirstOrDefaultAsync(m => m.Id == courseImage.CourseId);

            if (data.CourseImages.Count > 1)
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", courseImage.Image);

                FileHelper.DeleteFile(path);

                _context.CourseImages.Remove(courseImage);

                await _context.SaveChangesAsync();

                result = true;
            }

            data.CourseImages.FirstOrDefault().IsMain = true;

            await _context.SaveChangesAsync();

            return Ok(result);

        }





        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            ViewBag.owners = await GetOwnersAsync();

            Course dbCourse = await _context.Courses.Include(m => m.CourseImages).Include(m => m.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (dbCourse == null) return NotFound();


            CourseEditVM model = new()
            {
                Id = dbCourse.Id,
                Name = dbCourse.Name,
                Sales = dbCourse.Sales,
                Price = dbCourse.Price.ToString("0.#####"),
                OwnerId = dbCourse.OwnerId,
                Images = dbCourse.CourseImages.ToList(),
                Description = dbCourse.Description
            };


            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM updateCourse)
        {
            if (id == null) return BadRequest();

            ViewBag.owners = await GetOwnersAsync();

            Course dbCourse = await _context.Courses.AsNoTracking().Include(m => m.CourseImages).Include(m => m.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (dbCourse == null) return NotFound();

            if (!ModelState.IsValid)
            {
                updateCourse.Images = dbCourse.CourseImages.ToList();
                return View(updateCourse);
            }

            List<CourseImage> courseImages = new();

            if (updateCourse.Photos is not null)
            {
                foreach (var photo in updateCourse.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        updateCourse.Images = dbCourse.CourseImages.ToList();
                        return View(updateCourse);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        updateCourse.Images = dbCourse.CourseImages.ToList();
                        return View(updateCourse);
                    }
                }



                foreach (var photo in updateCourse.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    CourseImage courseImage = new()
                    {
                        Image = fileName
                    };

                    courseImages.Add(courseImage);
                }

                await _context.CourseImages.AddRangeAsync(courseImages);
            }

            decimal convertedPrice = decimal.Parse(updateCourse.Price.Replace(".", ","));

            Course newCourse = new()
            {
                Id = dbCourse.Id,
                Name = updateCourse.Name,
                Price = convertedPrice,
                Sales = updateCourse.Sales,
                Description = updateCourse.Description,
                OwnerId = updateCourse.OwnerId,
                CourseImages = courseImages.Count == 0 ? dbCourse.CourseImages : courseImages
            };


            _context.Courses.Update(newCourse);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
