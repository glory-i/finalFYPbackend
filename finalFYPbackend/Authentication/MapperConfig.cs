using AutoMapper;
using finalFYPbackend.Model.NutritionModels;
using finalFYPbackend.Model;
using System;
using finalFYPbackend.DTOs.MealDTOs;

namespace finalFYPbackend.Authentication
{
    public class MapperConfig
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SignUpModel, ValidateModel>();
                cfg.CreateMap<SignUpModel, CalculateCalorieRequirementsModel>();
                cfg.CreateMap<ApplicationUser, LoginResponseModel>();
                cfg.CreateMap<AuthorizationToken, LoginResponseModel>();
                cfg.CreateMap<UpdateUserModel, CalculateCalorieRequirementsModel>();


                //CREATES A MAP FROM nutritionrequestmodel to Calulcate calorie rewuirements model. It will return an instance of calculate calorie requirements model
                cfg.CreateMap<NutritionCalculatorRequestModel, CalculateCalorieRequirementsModel>();
                cfg.CreateMap<createMealDTO, Meal>();
                cfg.CreateMap<createFoodDTO, Food>();
            }
                   );
            return config;
        }
    }
}
