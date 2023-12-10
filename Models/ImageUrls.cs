using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduNext.Models
{
    public class ImageUrls
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string? ImgUrl { get; set; }
        public Student? Student { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
    }
}
