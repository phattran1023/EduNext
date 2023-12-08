using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduNext.Controllers
{
    public class CourseController : Controller
    {
        private readonly DatabaseContext _context;

        public CourseController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
    }
}
