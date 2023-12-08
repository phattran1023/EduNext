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
    }
}
