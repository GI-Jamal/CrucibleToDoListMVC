using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrucibleToDoListMVC.Data;
using CrucibleToDoListMVC.Models;
using Microsoft.AspNetCore.Identity;
using CrucibleToDoListMVC.Services.Interfaces;

namespace CrucibleToDoListMVC.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToDoListService _toDoListService;

        public ToDoItemsController(ApplicationDbContext context, UserManager<AppUser> userManager, IToDoListService toDoListService)
        {
            _context = context;
            _userManager = userManager;
            _toDoListService = toDoListService;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index(bool showCompletedTasks = false)
        {
            string? appUserId = _userManager.GetUserId(User);

            List<ToDoItem>? toDoItems = new List<ToDoItem>();

            if (showCompletedTasks == true)
            {
                toDoItems = await _context.ToDoItems.Where(t => t.AppUserId == appUserId).Where(t => t.IsCompleted == true).Include(t => t.Accessories).ToListAsync();

            }
            else
            {
                toDoItems = await _context.ToDoItems.Where(t => t.AppUserId == appUserId).Where(t => t.IsCompleted == false).Include(t => t.Accessories).ToListAsync();
            }


            return View(toDoItems);
        }

        // GET: ToDoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AccessoriesList"] = await GetAccessoriesListAsync();

            ToDoItem toDoItem = new ToDoItem();

            return View(toDoItem);
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AppUserId,DueDate,IsCompleted")] ToDoItem toDoItem, IEnumerable<int> selected)
        {
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {
                toDoItem.AppUserId = _userManager.GetUserId(User);
                toDoItem.DateCreated = DateTime.UtcNow;

                if (toDoItem.DueDate != null)
                {
                    toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate.Value, DateTimeKind.Utc);
                }

                _context.Add(toDoItem);
                await _context.SaveChangesAsync();


                await _toDoListService.AddAccessoriesToToDoItemAsync(selected, toDoItem.Id);


                return RedirectToAction(nameof(Index));
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            ToDoItem? toDoItem = await _context.ToDoItems.Include(t => t.Accessories).FirstOrDefaultAsync(t => t.Id == id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            ViewData["AccessoriesList"] = await GetAccessoriesListAsync(toDoItem.Accessories.Select(a => a.Id));

            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AppUserId,DateCreated,DueDate,IsCompleted")] ToDoItem toDoItem, IEnumerable<int> selected)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDoItem.DateCreated = DateTime.SpecifyKind(toDoItem.DateCreated, DateTimeKind.Utc);

                    if (toDoItem.DueDate != null)
                    {
                        toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate.Value, DateTimeKind.Utc);
                    }
                    
                    
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();

                    if (selected != null)
                    {
                        await _toDoListService.RemoveAccessoriesFromToDoItemAsync(toDoItem.Id);
                        await _toDoListService.AddAccessoriesToToDoItemAsync(selected, toDoItem.Id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AccessoriesList"] = await GetAccessoriesListAsync(toDoItem.Accessories.Select(a => a.Id));
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToDoItems'  is null.");
            }
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItems.Remove(toDoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
          return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<MultiSelectList> GetAccessoriesListAsync(IEnumerable<int> accessoryIds = null!)
        {
            string? appUserId = _userManager.GetUserId(User);

            IEnumerable<Accessory> accessories = await _context.Accessories.Where(a => a.AppUserId == appUserId).ToListAsync();

            return new MultiSelectList(accessories, "Id", "Name", accessoryIds);
        }

    }
}
