using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduNext.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly DatabaseContext _context;
        public EnrollmentController(DatabaseContext context) { _context = context; }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student);
            return View(await enrollments.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                // Add the enrollment to the database
                // You can use your DbContext to add and save changes
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(enrollment);
        }
    }
}
