using System.ComponentModel.DataAnnotations;
using System;

namespace finalFYPbackend.Authentication
{
    public class ValidateModel
    {
        //These are the properties to be validated before creating an account - Username, password, etc
        public string FirstName { get; set; }
        public string LastName { get; set; }


        //[Required(ErrorMessage = "USERNAME IS REQUIRED")]
        public string UserName { get; set; }


        //[Required(ErrorMessage = "EMAIL IS REQUIRED")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PASSWORD IS REQUIRED")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }


    public class SignUpModel
    {
        //These are the properties to be used to create an account 
        public string FirstName { get; set; }
        public string LastName { get; set; }


        [Required(ErrorMessage = "USERNAME IS REQUIRED")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "EMAIL IS REQUIRED")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PASSWORD IS REQUIRED")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }
        public double Weight { get; set; }
        public double HeightInFeet { get; set; }
        public double HeightInInches { get; set; }
        //public string HeightUnit { get; set; }
        public string ActivityLevel { get; set; }
        public string Goal { get; set; }

    }


    public class CalculateCalorieRequirementsModel
    {
        public int Age { get; set; }
        public double Weight { get; set; }
        public double HeightInFeet { get; set; }
        public double HeightInInches { get; set; }
        //public string HeightUnit { get; set; }
        public string ActivityLevel { get; set; }
        public string Goal { get; set; }
        public string Gender { get; set; }
    }


    public class LoginModel
    {
        //[Required(ErrorMessage = "Username OR Email is required")]
        public string UsernameOrEmail { get; set; }


        //[Required(ErrorMessage = "Paasword is required")]
        public string Password { get; set; }
    }

    public class AuthorizationToken
    {
        public string Token { get; set; }
        public string TokenUser { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class LoginResponseModel : ApplicationUser
    {
        public string Token { get; set; }
        public string TokenUser { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class UpdateUserModel
    {
        // public string FirstName { get; set; }
        // public string LastName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double HeightInFeet { get; set; }
        public double HeightInInches { get; set; }
        //public string HeightUnit { get; set; }
        public string ActivityLevel { get; set; }
        public string Goal { get; set; }
        //public string Gender { get; set; }
    }


    public class RegisterAdminModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int Age { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginAdminModel
    {
        public string usernameorEmail { get; set; }
        public string password { get; set; }
    }

}
