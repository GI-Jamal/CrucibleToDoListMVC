namespace CrucibleToDoListMVC.Services.Interfaces
{
    public interface IToDoListService
    {
        public Task AddAccessoriesToToDoItemAsync(IEnumerable<int> accessoryIds, int toDoItemId);

        public Task RemoveAccessoriesFromToDoItemAsync(int toDoItemId);
    }
}
