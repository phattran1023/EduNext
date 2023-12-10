using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduNext;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }
    [Required]
    public string Title { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }

}
