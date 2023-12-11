using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Import the System.Linq namespace for the SelectMany method
using System.Threading.Tasks;

namespace EduNext.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public DepartmentController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _databaseContext.Departments.ToListAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var newDepartment = new Department(); // Initialize a new Department instance
            return View(newDepartment);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _databaseContext.Departments.AddAsync(department);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _databaseContext.Departments.FindAsync(id);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _databaseContext.Departments.Update(department);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _databaseContext.Departments.FindAsync(id);
            _databaseContext.Departments.Remove(department);
            await _databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
