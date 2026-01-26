using System.ComponentModel.DataAnnotations;

namespace Engine.Models.ViewModels
{
    public class AppViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
