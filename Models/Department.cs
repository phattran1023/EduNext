using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduNext;

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentId { get; set; }
    public string Name { get; set; }
}

