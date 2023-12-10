using EduNext.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduNext;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentId { get; set; }
    [Required(ErrorMessage ="Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Department field is required.")]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }
    public List<ImageUrls>? ImageUrls { get; set; }
    [NotMapped]
    public IFormFile? ImgFile { get; set; }
}
