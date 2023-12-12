using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Edit([Bind("EnrollmentId, StudentId, CourseId")] Enrollment updatedEnrollment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingEnrollment = await _context.Enrollments
                        .FirstOrDefaultAsync(e => e.EnrollmentId == updatedEnrollment.EnrollmentId && e.EnrollmentId > 0);

                    if (existingEnrollment == null)
                    {
                        return NotFound();
                    }

                    // Check if values have changed
                    if (existingEnrollment.StudentId != updatedEnrollment.StudentId || existingEnrollment.CourseId != updatedEnrollment.CourseId)
                    {
                        // Create a new Enrollment entity with the updated values
                        var newEnrollment = new Enrollment
                        {
                            StudentId = updatedEnrollment.StudentId,
                            CourseId = updatedEnrollment.CourseId
                        };

                        // Remove the existing enrollment from the context
                        _context.Enrollments.Remove(existingEnrollment);

                        // Add the new enrollment to the context
                        _context.Enrollments.Add(newEnrollment);

                        // Save changes to persist the modifications
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"Error updating enrollment: {ex.Message}");

                    // Convert ModelState errors to a list of strings
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    TempData["Errors"] = errors;

                    return View(updatedEnrollment);
                }
            }

            // If ModelState is not valid, store errors in TempData
            var validationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            TempData["Errors"] = validationErrors;

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View(updatedEnrollment);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            // Find the enrollment based on the composite key
            var enrollmentToDelete = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollmentToDelete != null)
            {
                // Remove the enrollment from the context
                _context.Enrollments.Remove(enrollmentToDelete);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle the case where the enrollment with the given composite key is not found
                // This might be a NotFound situation
                return NotFound();
            }
        }

    }
}
