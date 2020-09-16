using Microsoft.EntityFrameworkCore;

namespace SPA.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User adminUser = new User()
            {
                Id = 1,
                Email = "admin@mail.ru",
                Login = "admin",
                Name = "Denis",
                Password = "123",
                Role = Roles.Admin
            };
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}