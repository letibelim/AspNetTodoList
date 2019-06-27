using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemAsync(TodoItem todoItem, Microsoft.AspNetCore.Identity.IdentityUser currentUser)
        {
            todoItem.Id = Guid.NewGuid();
            todoItem.IsDone = false;
            todoItem.UserId = currentUser.Id;
            todoItem.DueAt = DateTime.Now.AddDays(3);

            _context.Items.Add(todoItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync(Microsoft.AspNetCore.Identity.IdentityUser currentUser)
        {
            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == currentUser.Id)
                .ToArrayAsync();
        }

        public async Task<bool> MarkDoneAsync(Guid id, Microsoft.AspNetCore.Identity.IdentityUser currentUser)
        {
            var todoItem = await _context.Items
                .Where(x => x.Id == id && x.UserId == currentUser.Id)
                .SingleOrDefaultAsync();
                ;
            if( todoItem == null )
            {
                return false;
            }

            todoItem.IsDone = true;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}
