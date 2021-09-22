using Sample.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Impl.Services.ToDoTasks
{
    public interface ITasksService
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync();

        Task<ToDoItem> GetAsync(int? id);

        void DeleteAsync(int? id);

        void UpdateAsync(ToDoItem item);

        void CreateAsync(ToDoItem item);
    }
}
