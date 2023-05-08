using Microsoft.AspNetCore.Identity;

namespace finalFYPbackend.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public double Weight { get; set; }
        public double HeightInCm { get; set; }
        public double HeightInFeet { get; set; }
        public double HeightInInches { get; set; }
        public int ActivityLevel { get; set; }
        public string userActivityLevel { get; set; }
        public string Goal { get; set; }
        public double CalorieRequirement { get; set; }

    }
}
