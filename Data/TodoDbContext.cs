using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var i = 0;

            // Seed inicial dos dados.
            modelBuilder.Entity<Todo>()
                .HasData(TodoAppUser.AllowedUsers
                    .OrderBy(u => u, StringComparer.OrdinalIgnoreCase)
                    .SelectMany(u => new[]
                    {
                        new Todo { Id = ++i, AddedAt = DateTimeOffset.Now, Description = "Regar as plantas", IsComplete = false, LastUpdatedAt = DateTimeOffset.Now, UserName = u },
                        new Todo { Id = ++i, AddedAt = DateTimeOffset.Now, Description = "Dar de comer ao gato", IsComplete = true, LastUpdatedAt = DateTimeOffset.Now, UserName = u },
                        new Todo { Id = ++i, AddedAt = DateTimeOffset.Now, Description = "Estudar TI2", IsComplete = false, LastUpdatedAt = DateTimeOffset.Now, UserName = u },
                    })
                    .ToList()
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}