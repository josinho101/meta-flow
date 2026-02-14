using System.ComponentModel.DataAnnotations;

namespace Engine.Models.ViewModels
{
    public class DbMetadataViewModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9_-]*$", ErrorMessage = "Only alphabets, numbers, hyphens, and underscores are allowed.")]
        public string DbName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9_-]*$", ErrorMessage = "Only alphabets, numbers, hyphens, and underscores are allowed.")]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
