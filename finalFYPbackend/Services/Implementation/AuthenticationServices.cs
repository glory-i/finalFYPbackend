using AutoMapper;
using finalFYPbackend.Authentication;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System;
using finalFYPbackend.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using finalFYPbackend.Utilities;
using finalFYPbackend.Model.EmailSettings;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Microsoft.Extensions.Options;

namespace finalFYPbackend.Services.Implementation
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly EmailSettings _emailSettings;


        private ApplicationDbContext _context;
        public AuthenticationServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.roleManager = roleManager;
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        public async Task<string> CreateRoles()
        {
            //create the "User" Role
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                return "User Role successfully created";
            }

            //create the "Admin" Role
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                return "Admin Role successfully created";
            }
            return "Role already exists";


        }



        //REMEMBER TO CHANGE THIS
        public async Task<ApiResponse> RegisterUser(ValidateModel model)
        {
            //create the "user" role
            await CreateRoles();

            var validateEmail = await ValidateEmail(model.Email);
            if (!validateEmail)
            {
                ReturnedResponse returnedResponse = new ReturnedResponse();
                return returnedResponse.ErrorResponse("Email is Invalid", null);
            }

            //ensure no other user has the same username
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                ReturnedResponse returnedResponse = new ReturnedResponse();
                return returnedResponse.ErrorResponse("User with that Username Already Exists", null);
            }

            //ensure no other user has the same email
            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                ReturnedResponse returnedResponse = new ReturnedResponse();
                return returnedResponse.ErrorResponse("User with that Email Already Exists", null);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            return new ApiResponse
            {
                data = user,
                error = null,
                Message = ApiResponseEnum.success.ToString(),
                code = "200"

            };




            /*
            //create user and add the "user" role.
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ///RETURN A NEW API RESPONSE THAT THE USER WAS NOT CREATED

            }
            await userManager.AddToRoleAsync(user, UserRoles.User);

            */


            //create ACCOUNT instance with additional properties like height,age, gender, etc and save the account to the database.
            /*Owner owner = new Owner
            {
                Username = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateCreated = DateTime.Now
            };

            await _context.Owners.AddAsync(owner);
            */

            /// await _context.SaveChangesAsync();

            ///return new AuthenticationResponse { Status = AuthenticationResponseEnum.Success.GetEnumDescription(), Message = "User Created Succesfully" };


        }


        public async Task<ApiResponse> CheckValidations(ValidateModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            //create the "user" role
            await CreateRoles();

            if (String.IsNullOrEmpty(model.UserName))
            {
                return returnedResponse.ErrorResponse("Username cannot be empty", null);

            }

            if (String.IsNullOrEmpty(model.Email))
            {
                return returnedResponse.ErrorResponse("Email cannot be empty", null);

            }

            //ensure that the email is valid using regular expressions.
            var validateEmail = ValidateEmailRegExp(model.Email);
            if (!validateEmail)
            {
                return returnedResponse.ErrorResponse("Email is Invalid", null);
            }


            //ensure no other user has the same username
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                return returnedResponse.ErrorResponse("User with that Username Already Exists", null);
            }

            //ensure no other user has the same email
            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                return returnedResponse.ErrorResponse("User with that Email Already Exists", null);
            }

            //ensure password meets the validations
            var validatePassword = ValidatePassword(model.Password);
            if (validatePassword.error != null)
            {
                return returnedResponse.ErrorResponse(validatePassword.error.message, null);
            }

            //ensure password and confirm password are the same
            if (model.Password != model.ConfirmPassword)
            {
                return returnedResponse.ErrorResponse("Password and Confirm Password do not match", null);
            }


            return returnedResponse.CorrectResponse("Valid Details");

        }

        //SEEMS THIS MAY BE USELESS
        public async Task<bool> ValidateEmail(string email)
        {
            bool isEmailValid = false;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://zerobounce1.p.rapidapi.com/v2/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add($"X-RapidAPI-Key", $"26c54984bcmsh448ceed8e8e9cecp1e9f68jsn5dc0e458b769");
            client.DefaultRequestHeaders.Add($"X-RapidAPI-Host", $"zerobounce1.p.rapidapi.com");

            string apiKey = "api_key=5b63a372c59744d7a09150493eb744d9";
            string path = $"valdate?{apiKey}&email={email}";

            try
            {
                HttpResponseMessage Res = await client.GetAsync(path);

                if (Res.IsSuccessStatusCode)
                {
                    var emailValidationResponse = await Res.Content.ReadFromJsonAsync<EmailValidationResponse>();
                    if (emailValidationResponse.status == EmailValidationEnum.valid.ToString())
                    {
                        isEmailValid = true;
                    }
                }

                return isEmailValid;
            }

            catch (Exception my_ex)
            {
                ///refine later
                throw new BadHttpRequestException(my_ex.Message);
            }


        }

        public ApiResponse ValidatePassword(string password)
        {

            string errorMessage = "";

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniChars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            ReturnedResponse returnedResponse = new ReturnedResponse();

            if (!hasLowerChar.IsMatch(password))
            {
                errorMessage = "Password should contain At least one lower case letter";
                return returnedResponse.ErrorResponse(errorMessage, null);
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                errorMessage = "Password should contain At least one upper case letter";
                return returnedResponse.ErrorResponse(errorMessage, null);
            }
            else if (!hasMiniChars.IsMatch(password))
            {
                errorMessage = "Password should not be less than 8 characters";
                return returnedResponse.ErrorResponse(errorMessage, null);
            }
            else if (!hasNumber.IsMatch(password))
            {
                errorMessage = "Password should contain At least one numeric value";
                return returnedResponse.ErrorResponse(errorMessage, null);
            }

            else if (!hasSymbols.IsMatch(password))
            {
                errorMessage = "Password should contain At least one special case characters";
                return returnedResponse.ErrorResponse(errorMessage, null);
            }
            else
            {
                return returnedResponse.CorrectResponse(true);
            }
        }

        public bool ValidateEmailRegExp(string email)
        {
            var validEmail = new Regex("^\\S+@\\S+\\.\\S+$");
            if (validEmail.IsMatch(email))
            {
                return true;
            }
            return false;

        }

        public async Task<ApiResponse> CreateOTP(string username, string email)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            try
            {
                Random random = new Random();
                int pin = random.Next(10000, 99999);

                var newOTP = new OTP
                {
                    pin = pin,
                    username = username,
                    email = email,
                    dateExpired = DateTime.Now.AddMinutes(1),

                };
                await _context.OTPs.AddAsync(newOTP);
                await _context.SaveChangesAsync();

                return returnedResponse.CorrectResponse(newOTP.pin);
            }

            catch (Exception myEx)
            {
                return returnedResponse.ErrorResponse(myEx.ToString(), null);
            }




        }

        /* public async Task<ApiResponse> SendOTP(string username, string email)
         {
             ReturnedResponse returnedResponse = new ReturnedResponse();
             var newOTP = await CreateOTP(username, email);

             if (newOTP.error != null)
             {
                 return returnedResponse.ErrorResponse(newOTP.error.message, null);
             }

             HttpClient client = new HttpClient();
             string baseUrl = "https://rapidprod-sendgrid-v1.p.rapidapi.com/";

             client.BaseAddress = new Uri(baseUrl);
             client.DefaultRequestHeaders.Clear();
             client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
             client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "26c54984bcmsh448ceed8e8e9cecp1e9f68jsn5dc0e458b769");
             client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "rapidprod-sendgrid-v1.p.rapidapi.com");

             SendEmailRequest request = new SendEmailRequest
             {
                 personalizations = new List<Personalization>()
                 {
                     new Personalization
                     {
                         to = new List<To>()
                         {
                             new To
                             {
                                 email = email,
                             }
                         },
                         subject = "YOUR ONE TIME PASSOWRD (OTP)",
                     }
                 },

                 from = new From
                 {
                     email = "gniweriebor@gmail.com",
                 },

                 content = new List<Content>()
                 {
                     new Content
                     {
                         type = "text/plain",
                          value = $"YOUR ONE TIME PASSWORD TO LOG IN TO FOODIFIED HAS BEEN GENERATED. DO NOT GIVE ANYONE, IT IS {newOTP.data}",
                     }
                 }

             };

             try
             {
                 string path = "mail/send";
                 HttpResponseMessage Res = await client.PostAsJsonAsync(path, request);

                 if (Res.IsSuccessStatusCode)
                 {
                     var response = await Res.Content.ReadAsStringAsync();
                     return returnedResponse.CorrectResponse("Email Successfully Sent");

                 }

                 return returnedResponse.ErrorResponse(Res.Content.ToString(), null);

             }

             catch (Exception my_ex)
             {
                 return returnedResponse.ErrorResponse(my_ex.Message.ToString(), null);

             }


         }

         */
        public async Task<ApiResponse> SendOTP(string username, string email)
        {


            ReturnedResponse returnedResponse = new ReturnedResponse();
            var senderEmail = _emailSettings.emailAddress;
            var senderPassword = _emailSettings.password;

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            var newOTP = await CreateOTP(username, email);

            if (newOTP.error != null)
            {
                return returnedResponse.ErrorResponse(newOTP.error.message, null);
            }

            try
            {
                MailMessage mailMessage = new MailMessage(senderEmail, email);
                mailMessage.Subject = "ONE-TIME PASSWORD FROM FOODIFIED";
                mailMessage.Body = $"Hello {username}, Please kindly find your one time password for FOODIFIED Below :" +
                    $"\n  {newOTP.data} Thank you for using FOODIFIED";


                await client.SendMailAsync(mailMessage);
                return returnedResponse.CorrectResponse("successfully sent message");

            }

            catch (Exception my_ex)
            {
                return returnedResponse.ErrorResponse(my_ex.Message.ToString(), null);

            }


        }


        public async Task<ApiResponse> ValidateOTP(string inputPin, string username, string email)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            if (String.IsNullOrEmpty(inputPin))
            {
                return returnedResponse.ErrorResponse("OTP cannot be empty", null);
            }

            var userOTP = await _context.OTPs.Where(o => o.email == email).OrderBy(o => o.Id).LastAsync();
            
            if (userOTP.pin == Convert.ToInt32(inputPin))
            {
                if (DateTime.Now > userOTP.dateExpired)
                {
                    return returnedResponse.ErrorResponse("This otp has expired", null);
                }

                return returnedResponse.CorrectResponse("OTP successfully validated");
            }
            return returnedResponse.ErrorResponse("Invalid OTP", null);
        }

        public async Task<double> CalculateHeight(double feet, double inches)
        {
            //this method is to get the height in cm.

            double height = 0.0;
            double feetToInches = 12.0;
            double inchesToCm = 2.54;

            double metersToCm = 100.0;

            //IF height given in feet and inches, convert everything to inches first, then convert to cm by multiplying by 2.54
            double heightInInches = (feet * feetToInches) + inches;
            height = heightInInches * inchesToCm;

            height = Math.Round(height, 2);

            return await Task.FromResult(height);
        }

        public async Task<double> CalculateBMR(CalculateCalorieRequirementsModel model)
        {
            //this method is to calculate BMR using the Mifflin St-Jeor Equation.

            //firstly get the height in cm
            double heightInCm = await CalculateHeight(model.HeightInFeet, model.HeightInInches);


            //the Mifflin St-Jeor formula is the same for males and females and only differs by the constant added at the end
            double constantForFemales = -161.0;
            double constantForMales = +5.0;

            double constant = model.Gender == GenderEnum.Male.GetEnumDescription() ? constantForMales : constantForFemales;


            //mifflin-stJeor formula for calculating BMR
            double BMR = (10.0 * model.Weight) + (6.25 * heightInCm) - (5.0 * model.Age) + constant;


            //round to 2 dp
            BMR = Math.Round(BMR, 2);

            return await Task.FromResult(BMR);

        }

        public async Task<double> CalculateAMR(CalculateCalorieRequirementsModel model)
        {
            //calculate the BMR using the Mifflin St-Jeor Equation 
            double BMR = await CalculateBMR(model);
            double activityFactor = 0.0;


            int userActivityLevel = AssignActivityLevel(model.ActivityLevel);
            switch (userActivityLevel)
            {
                case 1:
                    activityFactor = 1.2;
                    break;
                case 2:
                    activityFactor = 1.375;
                    break;
                case 3:
                    activityFactor = 1.55;
                    break;
                case 4:
                    activityFactor = 1.725;
                    break;
                case 5:
                    activityFactor = 1.9;
                    break;
            }

            //calculate the AMR and return it
            double AMR = activityFactor * BMR;
            return await Task.FromResult(AMR);

        }

        public async Task<double> CalculateCalorieRequirements(CalculateCalorieRequirementsModel model)
        {
            //method for determining calorie requirements.
            double calorieRequirements = 0.0;

            //calculate the AMR
            double AMR = await CalculateAMR(model);

            //I am using 20% for the calorie deficit/ calorie gain.
            double difference = (20.0 / 100.0) * (AMR);

            //add/subtract to the AMR based on the goal of the user.
            if (model.Goal == GoalEnum.Gain.GetEnumDescription()) calorieRequirements = AMR + difference;
            if (model.Goal == GoalEnum.Lose.GetEnumDescription()) calorieRequirements = AMR - difference;
            if (model.Goal == GoalEnum.Maintain.GetEnumDescription()) calorieRequirements = AMR;

            calorieRequirements = Math.Round(calorieRequirements, 2);

            //return the calorie requirements
            return await Task.FromResult(calorieRequirements);
        }

        public int AssignActivityLevel(string activityLevel)
        {
            //this method is for assigning activity level to be a number.


            int userActivityLevel = 0;

            if (activityLevel == ActivityLevelEnum.Sedentary.GetEnumDescription()) userActivityLevel = 1;
            if (activityLevel == ActivityLevelEnum.LightlyActive.GetEnumDescription()) userActivityLevel = 2;
            if (activityLevel == ActivityLevelEnum.ModeratelyActive.GetEnumDescription()) userActivityLevel = 3;
            if (activityLevel == ActivityLevelEnum.Active.GetEnumDescription()) userActivityLevel = 4;
            if (activityLevel == ActivityLevelEnum.VeryActive.GetEnumDescription()) userActivityLevel = 5;

            return userActivityLevel;

        }

        public async Task<ApiResponse> SignUpUser(SignUpModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            //create the "user" role
            await CreateRoles();

            var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
            var validateModel = mapper.Map<ValidateModel>(model);
            var calculateCalorieRequirementsModel = mapper.Map<CalculateCalorieRequirementsModel>(model);


            //USE AUTOMAPPER TO DO THESE PLEASE
            /*ValidateModel validateModel = new ValidateModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };
            */


            /*CalculateCalorieRequirementsModel calculateCalorieRequirementsModel = new CalculateCalorieRequirementsModel
            {
                Age = model.Age,
                Weight = model.Weight,
                Height1 = model.Height1,
                Height2 = model.Height2,
                HeightUnit = model.HeightUnit,
                ActivityLevel = model.ActivityLevel,
                Goal = model.Goal,
                Gender = model.Gender
            };
            */


            //just for safety, validate everything again just to be sure
            var validations = await CheckValidations(validateModel);


            if (validations.Message != ApiResponseEnum.success.ToString())
            {
                return returnedResponse.ErrorResponse(validations.error.message, null);
            }

            //create the application user instance

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Weight = model.Weight,
                HeightInCm = await CalculateHeight(model.HeightInFeet, model.HeightInInches),
                HeightInFeet = model.HeightInFeet,
                HeightInInches = model.HeightInInches,
                Gender = model.Gender == GenderEnum.Male.GetEnumDescription() ? GenderEnum.Male.GetEnumDescription() : GenderEnum.Female.GetEnumDescription(),
                ActivityLevel = AssignActivityLevel(model.ActivityLevel),
                userActivityLevel = model.ActivityLevel,
                Goal = model.Goal,
                CalorieRequirement = await CalculateCalorieRequirements(calculateCalorieRequirementsModel),

            };

            //use the user manager to create the user in the database
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return returnedResponse.ErrorResponse("User Not Created", null);
            }

            //create a user role for the user and save changes to the database
            await userManager.AddToRoleAsync(user, UserRoles.User);
            await _context.SaveChangesAsync();
            return returnedResponse.CorrectResponse(user);

        }

        public async Task<ApiResponse> Login(LoginModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            if (String.IsNullOrEmpty(model.UsernameOrEmail))
            {
                return returnedResponse.ErrorResponse("Username cannot be empty", null);

            }


            //check if there is any user with that email or username
            var user = await userManager.FindByNameAsync(model.UsernameOrEmail) ?? await userManager.FindByEmailAsync(model.UsernameOrEmail);

            //if user does not exist, return an error response
            if (user == null) return returnedResponse.ErrorResponse("No User exists with that Username or Email", null);


            //check if the password used is correct
            bool correctPassword = await userManager.CheckPasswordAsync(user, model.Password);

            //if password is incorrect, return an error response
            if (!correctPassword) return returnedResponse.ErrorResponse("Incorrect Login Details", null);




            //if the user is not null and the password is correct, assign roles and claims to the user

            try
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                //generate the JSON Web Token for the User
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


                //create the Token Object for the JSON Web Token
                AuthorizationToken authorizationToken = new AuthorizationToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    TokenUser = user.UserName
                };


                //use automapper to return all the user's information, along with their generated JWT
                var mapper = new Mapper(MapperConfig.GetMapperConfiguration());


                var loginResponseModel = mapper.Map<LoginResponseModel>(user);
                loginResponseModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
                loginResponseModel.Expiration = token.ValidTo;
                loginResponseModel.TokenUser = user.UserName;


                return returnedResponse.CorrectResponse(loginResponseModel);

            }

            catch (Exception myEx)
            {
                return returnedResponse.ErrorResponse(myEx.Message, null);
            }




        }

        public async Task<ApiResponse> UpdateUser(string username, UpdateUserModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            //check if there is any user with that email or username
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);

            //if user does not exist, return an error response
            if (user == null) return returnedResponse.ErrorResponse("No User exists with that Username or Email", null);


            //automapper which will be used for calculating the calorie requirements
            var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
            var calculateCalorieRequirementsModel = mapper.Map<CalculateCalorieRequirementsModel>(model);



            try
            {
                // update the values of the user
                //user.FirstName = model.FirstName;
                //user.LastName = model.LastName;
                user.Age = model.Age;
                user.Weight = model.Weight;
                user.HeightInCm = await CalculateHeight(model.HeightInFeet, model.HeightInInches);
                user.HeightInFeet = model.HeightInFeet;
                user.HeightInInches = model.HeightInInches;
                //user.Gender = model.Gender == GenderEnum.Male.GetEnumDescription() ? GenderEnum.Male.GetEnumDescription() : GenderEnum.Female.GetEnumDescription();
                user.ActivityLevel = AssignActivityLevel(model.ActivityLevel);
                user.userActivityLevel = model.ActivityLevel;
                user.Goal = model.Goal;
                user.CalorieRequirement = await CalculateCalorieRequirements(calculateCalorieRequirementsModel);

                await userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();

                return returnedResponse.CorrectResponse(user);

            }

            catch (Exception myEx)
            {
                return returnedResponse.ErrorResponse(myEx.Message, null);
            }


        }

        public async Task<ApiResponse> ValidateUserExists(string email)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            //check if there is any user with that email or username
            var user = await userManager.FindByNameAsync(email) ?? await userManager.FindByEmailAsync(email);

            //if user does not exist, return an error response
            if (user == null)
            {
                return returnedResponse.ErrorResponse("No User exists with that Username or Email", null);
            }

            else
            {
                return returnedResponse.CorrectResponse(user);
            }



        }

        public async Task<ApiResponse> ForgotPassword(string email, string newPassword, string confirmPassword)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            // check if there is any user with that email or username
            var user = await userManager.FindByEmailAsync(email);

            //if user does not exist, return an error response
            if (user == null)
            {
                return returnedResponse.ErrorResponse("No User exists with that  Email", null);
            }


            //ensure that the new password to be set is valid using regexp
            var validPassword = ValidatePassword(newPassword);
            if (validPassword.Message == ApiResponseEnum.failure.ToString())
            {
                return returnedResponse.ErrorResponse(validPassword.error.message, null);
            }


            //ensure that the password and confirm passwords match
            if (newPassword != confirmPassword)
            {
                return returnedResponse.ErrorResponse("Password and Confirm Password do not match", null);
            }


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return returnedResponse.CorrectResponse("Successfully changed Passwords");
            }
            return returnedResponse.ErrorResponse(result.Errors.ToString(), null);


        }

        public async Task<ApiResponse> ChangePassword(string username, string currentPassword, string newPassword, string confirmPassword)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            // check if there is any user with that email or username
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);

            //if user does not exist, return an error response
            if (user == null)
            {
                return returnedResponse.ErrorResponse("No User exists with that Username or Email", null);
            }


            //check if the password used is correct
            bool correctPassword = await userManager.CheckPasswordAsync(user, currentPassword);

            //if password is incorrect, return an error response
            if (!correctPassword) return returnedResponse.ErrorResponse("Incorrect Password", null);

            var changePassword = await ForgotPassword(username, newPassword, confirmPassword);

            if (changePassword.Message == ApiResponseEnum.failure.ToString())
            {
                return returnedResponse.ErrorResponse(changePassword.error.message, null);
            }

            return returnedResponse.CorrectResponse("Successfully changed Password");


        }

        public async Task<ApiResponse> ViewUser(string username)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            // check if there is any user with that email or username
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);

            //if user does not exist, return an error response
            if (user == null)
            {
                return returnedResponse.ErrorResponse("No User exists with that Username or Email", null);
            }

            return returnedResponse.CorrectResponse(user);
        }

        public async Task<ApiResponse> RegisterAdmin(RegisterAdminModel model)
        {
            //create the "user and admin" role
            await CreateRoles();

            ReturnedResponse returnedResponse = new ReturnedResponse();
            if(model.username != AdminConstants.username)
            {
                return returnedResponse.ErrorResponse("Incorrect admin details", null);
            }

            var admin = await userManager.FindByNameAsync(model.username) ?? await userManager.FindByEmailAsync(model.email);
            if (admin != null)
            {
                return returnedResponse.ErrorResponse("Admin already exists", null);
            }

            ApplicationUser newAdmin = new ApplicationUser
            {
                UserName = model.username,
                Email = model.email,
                FirstName = model.firstName,
                LastName = model.lastName,
                Age = model.Age,
            };
           
            //use the user manager to create the admin in the database
            var result = await userManager.CreateAsync(newAdmin, model.password);

            //create a user role for the user and save changes to the database
            await userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
            await _context.SaveChangesAsync();
            return returnedResponse.CorrectResponse(newAdmin);

        }

        public async Task<ApiResponse> LoginAdmin(LoginAdminModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            if (String.IsNullOrEmpty(model.usernameorEmail))
            {
                return returnedResponse.ErrorResponse("Username cannot be empty", null);

            }


            //check if there is any user with that email or username
            var admin = await userManager.FindByNameAsync(model.usernameorEmail) ?? await userManager.FindByEmailAsync(model.usernameorEmail);

            //if user does not exist, return an error response
            if (admin == null) return returnedResponse.ErrorResponse("No Admin exists with that Username or Email", null);


            //check if the password used is correct
            bool correctPassword = await userManager.CheckPasswordAsync(admin, model.password);

            //if password is incorrect, return an error response
            if (!correctPassword) return returnedResponse.ErrorResponse("Incorrect Login Details", null);



            //if the user is not null and the password is correct, assign roles and claims to the user

            try
            {
                var userRoles = await userManager.GetRolesAsync(admin);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,admin.UserName),
                    new Claim(ClaimTypes.Email,admin.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                //generate the JSON Web Token for the User
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


                //create the Token Object for the JSON Web Token
                AuthorizationToken authorizationToken = new AuthorizationToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    TokenUser = admin.UserName
                };


                //use automapper to return all the admin's information, along with their generated JWT
                var mapper = new Mapper(MapperConfig.GetMapperConfiguration());


                var loginResponseModel = mapper.Map<LoginResponseModel>(admin);
                loginResponseModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
                loginResponseModel.Expiration = token.ValidTo;
                loginResponseModel.TokenUser = admin.UserName;


                return returnedResponse.CorrectResponse(loginResponseModel);

            }

            catch (Exception myEx)
            {
                return returnedResponse.ErrorResponse(myEx.Message, null);
            }



        }
    }
}

