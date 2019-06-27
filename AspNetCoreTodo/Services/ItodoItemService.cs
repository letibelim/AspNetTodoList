using AspNetCoreTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser currentUser);
        Task<bool> AddItemAsync(TodoItem todoItem, IdentityUser currentUser);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser currentUser);
    }


}
