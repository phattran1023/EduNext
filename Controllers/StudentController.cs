using EduNext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduNext.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public StudentController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index()
        {
            var student = await _databaseContext.Students.ToListAsync();
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch the departments from the database
            var departments = await _databaseContext.Departments.ToListAsync();

            // Create a ViewModel and pass it to the view
            var viewModel = new CreateStudentViewModel
            {
                Departments = departments,
                Student = new Student() // You can initialize other properties as needed
            };
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Errors"] = errors;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // If the model state is valid, add the student to the database
                await _databaseContext.Students.AddAsync(viewModel.Student);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If there are validation errors, fetch the departments from the database
            var departments = await _databaseContext.Departments.ToListAsync();

            // Update the ViewModel with the current student and departments
            viewModel.Departments = departments;

            // Convert ModelState errors to a list of strings
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            TempData["Errors"] = errors;

            return View(viewModel);
        }


    }
}