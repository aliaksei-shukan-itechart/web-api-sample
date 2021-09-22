using Sample.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Impl.Services.ToDoTasks
{
    public interface ITasksService
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync();

        Task<ToDoItem> GetAsync(int? id);

        Task DeleteAsync(int? id);

        Task UpdateAsync(int? id, ToDoItem item);

        Task CreateAsync(ToDoItem item);
    }
}
