using System.ComponentModel.DataAnnotations;

namespace EduNext.Models
{
	public class StudentEditViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Department is required")]
		public int DepartmentId { get; set; }
	}
}
