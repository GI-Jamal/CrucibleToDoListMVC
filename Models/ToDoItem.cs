using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrucibleToDoListMVC.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AppUserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Navigation Properties
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Accessory> Accessories { get; set; } = new HashSet<Accessory>();
    }
}
