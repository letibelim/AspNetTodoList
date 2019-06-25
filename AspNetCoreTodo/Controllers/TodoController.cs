using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        private ITodoItemService _todoItemService;
        public async Task<IActionResult> IndexAsync()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            return View();
        }
    }
}