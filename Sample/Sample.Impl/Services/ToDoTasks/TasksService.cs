using Microsoft.EntityFrameworkCore;
using Sample.DAL;
using Sample.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Impl.Services.ToDoTasks
{
    public class TasksService : ITasksService
    {
        private readonly DataContext _context;

        public TasksService(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ToDoItem item)
        {
            if (item == null)
            {
                throw new System.Exception("You are trying to create an empty entity!");
            }

            _context.ToDoItems.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue)
            {
                throw new System.Exception("Requested id is null!");
            }

            var item = await _context.ToDoItems.FindAsync(id);

            if (item == null)
            {
                throw new System.Exception("You are trying to delete not existing entity!");
            }

            _context.ToDoItems.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        public async Task<ToDoItem> GetAsync(int? id)
        {
            if (!id.HasValue)
            {
                throw new System.Exception("Requested id is null!");
            }

            var item = await _context.ToDoItems.FindAsync(id);

            if (item == null)
            {
                throw new System.Exception("You are trying to get not existing entity!");
            }

            return item;
        }

        public async Task UpdateAsync(int? id, ToDoItem item)
        {
            if (id == null)
            {
                throw new System.Exception("Requested id is null!");
            }

            var updatingItem = await _context.ToDoItems.FindAsync(id);

            if (updatingItem == null)
            {
                throw new System.Exception("You are trying to update not existing entity!");
            }

            updatingItem.Title = item.Title;
            updatingItem.Description = item.Description;
            updatingItem.ExpirationTime = item.ExpirationTime;
            updatingItem.IsCompleted = item.IsCompleted;

            await _context.SaveChangesAsync();
        }

        public async Task CheckExpirationTimeAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var item in await _context.ToDoItems.ToListAsync())
                {
                    if (item.IsCompleted == false && item.ExpirationTime.ToUniversalTime() <= System.DateTime.UtcNow)
                    {
                        item.IsCompleted = true;
                    }
                }

                await _context.SaveChangesAsync();

                await Task.Delay(5000, token);
            }
        }
    }
}
