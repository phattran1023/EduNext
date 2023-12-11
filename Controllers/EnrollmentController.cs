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

        [HttpGet]
        public async Task<IActionResult> Edit(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
            {
                return NotFound();
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int studentId, int courseId, [Bind("StudentId", "CourseId")] Enrollment updatedEnrollment)
        {
            if (studentId != updatedEnrollment.StudentId || courseId != updatedEnrollment.CourseId)
            {
                return NotFound();
            }

            ModelState.Remove("EnrollmentId"); // Exclude EnrollmentId from validation

            if (ModelState.IsValid)
            {
                // Retrieve the existing enrollment from the database
                var existingEnrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

                if (existingEnrollment == null)
                {
                    return NotFound();
                }

                // Update the existing enrollment using the updatedEnrollment values
                existingEnrollment.StudentId = updatedEnrollment.StudentId;
                existingEnrollment.CourseId = updatedEnrollment.CourseId;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(updatedEnrollment);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
