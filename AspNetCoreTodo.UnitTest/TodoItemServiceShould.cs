using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreTodo.UnitTest
{
    public class TodoItemServiceShould
    {
        private DbContextOptions<ApplicationDbContext> _options;

        public TodoItemServiceShould()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_DB").Options;
        }

        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
       
            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new IdentityUser
                {
                    Id = "fake-002",
                    UserName = "fake@example.com"
                };

                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing1"
                }, fakeUser);
            }

            using (var context = new ApplicationDbContext(_options))
            {
                /*var itemsInDatabase = await context
                    .Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);*/

                var item = await context.Items.FirstAsync(a => a.Title == "Testing1");
                Assert.Equal("Testing1", item.Title);
                Assert.False(item.IsDone);

                // Item should be due 3 days from now (give or take a second)
                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                Assert.True(difference < TimeSpan.FromSeconds(1));
                var count = await context.Items.CountAsync();
                Console.WriteLine("count" + count.ToString());
            }

        }
        [Fact]
        public async Task MarkDoneAsyncReturnsFalseIfIdDoesNotExist()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };

                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing2"
                }, fakeUser);

            }

            using (var context = new ApplicationDbContext(_options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };
                var fakeUser2 = new IdentityUser
                {
                    Id = "fake-001",
                    UserName = "fake@example.com"
                };

                var item = await context.Items.FirstAsync(a => a.Title == "Testing2");
                var count = await context.Items.CountAsync();
                Console.WriteLine("count" + count.ToString());
                Assert.False(await service.MarkDoneAsync(Guid.NewGuid(), fakeUser));
                Assert.False(await service.MarkDoneAsync(item.Id, fakeUser2));
                Assert.True(await service.MarkDoneAsync(item.Id, fakeUser));
            }
        }
    }
}
