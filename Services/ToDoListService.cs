using CrucibleToDoListMVC.Data;
using CrucibleToDoListMVC.Models;
using CrucibleToDoListMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrucibleToDoListMVC.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly ApplicationDbContext _context;

        public ToDoListService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAccessoriesToToDoItemAsync(IEnumerable<int> accessoryIds, int toDoItemId)
        {
            try
            {
                ToDoItem? toDoItem = await _context.ToDoItems.Include(t => t.Accessories).FirstOrDefaultAsync(t => t.Id == toDoItemId);
                foreach (int accesoryId in accessoryIds)
                {
                    Accessory? accessory = await _context.Accessories.FindAsync(accesoryId);

                    if (toDoItem != null && accessory != null)
                    {
                        toDoItem.Accessories.Add(accessory);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveAccessoriesFromToDoItemAsync(int toDoItemId)
        {
            try
            {
                ToDoItem? toDoItem = await _context.ToDoItems.Include(t => t.Accessories).FirstOrDefaultAsync(t => t.Id == toDoItemId);

                toDoItem!.Accessories.Clear();
                _context.Update(toDoItem);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
