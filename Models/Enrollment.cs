using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace EduNext;

public class Enrollment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EnrollmentId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [ForeignKey("StudentId")]
    public Student Student { get; set; }

    [ForeignKey("CourseId")]
    public Course Course { get; set; }
}


