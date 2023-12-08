using System.ComponentModel.DataAnnotations;

namespace EduNext.Models
{
    public class CreateStudentViewModel
    {
        public List<Department>? Departments { get; set; }
        public Student? Student { get; set; }
    }

}
