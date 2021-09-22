using System;

namespace Sample.DAL.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ExpirationTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}
