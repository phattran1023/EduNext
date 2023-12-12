using System.ComponentModel.DataAnnotations;

namespace EduNext.Models
{
    public class StudentIndexViewModel
    {
        public List<Department>? Departments { get; set; }
        public IEnumerable<Student>? Student { get; set; }
    }

}
