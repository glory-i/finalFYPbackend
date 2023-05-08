using finalFYPbackend.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace finalFYPbackend.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<OTP> OTPs { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<FoodifiedMeal> FoodifiedMeals { get; set; }
        public DbSet<Food> Foods { get; set; }
        

    }
}
