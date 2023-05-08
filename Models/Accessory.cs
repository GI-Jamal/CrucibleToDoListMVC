using System.ComponentModel.DataAnnotations;

namespace CrucibleToDoListMVC.Models
{
    public class Accessory
    {
        public int Id { get; set; }

        // Foreign Key
        [Required]
        [Display(Name = "Accessory Name")]
        public string? Name { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        // Navigation Properties
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new HashSet<ToDoItem>();
    }
}
