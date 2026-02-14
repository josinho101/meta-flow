using System.ComponentModel.DataAnnotations;

namespace Engine.Models.ViewModels
{
    public class DbMetadataViewModel
    {
        [Required]
        public string DbName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
