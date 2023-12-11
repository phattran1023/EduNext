using EduNext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduNext.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentController(DatabaseContext databaseContext, IWebHostEnvironment hostEnvironment)
        {
            _databaseContext = databaseContext;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var student = await _databaseContext.Students
                .Include(s => s.ImageUrls)
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToListAsync();
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
        public async Task<IActionResult> UploadImg(int id)
        {
            var student = await _databaseContext.Students.FindAsync(id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImg(Student student, List<IFormFile>? imgFile)
        {
            if (imgFile == null || imgFile.Count == 0)
            {
                ModelState.AddModelError("ImgFile", "Please choose a file!");
                return View(student);
            }
            foreach (var file in imgFile)
            {
                string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + file.FileName;
                //upload file vao thu muc wwwroot/images
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var imgUrl = "uploads/" + uniqueFileName;

                var newImgUrl = new ImageUrls
                {
                    StudentId = student.StudentId,
                    ImgUrl = imgUrl
                };
                _databaseContext.ImageUrls.Add(newImgUrl);
            }

            await _databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditUploadImg(int id)
        {
            var student = await _databaseContext.Students.Include(s=>s.ImageUrls).FirstOrDefaultAsync(s => s.StudentId == id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> EditUploadImg(Student student, List<IFormFile>? imgFile)
        {
            var stId = student.StudentId;
            var currentImageUrl = await _databaseContext.ImageUrls.Where(s => s.StudentId == stId).ToListAsync();

            List<ImageUrls> listNewImgUrl = new List<ImageUrls>();

            if (imgFile != null)
            {
                if (currentImageUrl.Count > 0)
                {
                    foreach (var file in currentImageUrl)
                    {
                        this.DeleteFile(file.ImgUrl);
                    }
                }

                foreach (var file in imgFile)
                {
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + file.FileName;
                    //upload file vao thu muc wwwroot/images
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                    string fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var imgUrl = "uploads/" + uniqueFileName;

                    var newImgUrl = new ImageUrls
                    {
                        StudentId = student.StudentId,
                        ImgUrl = imgUrl
                    };
                    listNewImgUrl.Add(newImgUrl);

                    _databaseContext.ImageUrls.Add(newImgUrl);
                }
                foreach (var item in currentImageUrl)
                {
                    _databaseContext.ImageUrls.Remove(item);
                }
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var item in listNewImgUrl)
                {
                    this.DeleteFile(item.ImgUrl);
                }
                foreach (var item in currentImageUrl)
                {
                    _databaseContext.ImageUrls.Remove(item);
                }
                await _databaseContext.SaveChangesAsync();
                return View(student);
            }            
        }
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            var student = await _databaseContext.Students.FindAsync(id);
            var departments = await _databaseContext.Departments.ToListAsync();
            var viewModel = new CreateStudentViewModel
            {
                Departments = departments,
                Student = student
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateStudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _databaseContext.Students.Update(viewModel.Student);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var departments = await _databaseContext.Departments.ToListAsync();
            viewModel.Departments = departments;
            return View(viewModel);
        }
    }
}