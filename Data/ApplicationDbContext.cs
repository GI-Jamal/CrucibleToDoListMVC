using CrucibleToDoListMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrucibleToDoListMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<ToDoItem> ToDoItems { get; set; } = default!;
        public virtual DbSet<Accessory> Accessories { get; set; } = default!;
    }
}