using AutoMapper;
using finalFYPbackend.Authentication;
using finalFYPbackend.Model.NutritionModels;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Interface;
using System.Threading.Tasks;
using System;
using finalFYPbackend.Utilities;

namespace finalFYPbackend.Services.Implementation
{
    public class NutritionServices : INutritionServices
    {
        private ApplicationDbContext _context;
        private IAuthenticationServices _authenticationServices;


        public NutritionServices(ApplicationDbContext context, IAuthenticationServices authenticationServices)
        {
            _context = context;
            _authenticationServices = authenticationServices;
        }

        public async Task<ApiResponse> NutritionCalculator(NutritionCalculatorRequestModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            //automapper which will be used for calculating the calorie requirements
            var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
            var calculateCalorieRequirementsModel = mapper.Map<CalculateCalorieRequirementsModel>(model);

            var requiredCalories = await _authenticationServices.CalculateCalorieRequirements(calculateCalorieRequirementsModel);

            //var requiredCalories = 2000.0;

            NutritionCalculatorResponseModel nutritionCalculatorResponseModel = new NutritionCalculatorResponseModel
            {

                totalCaloriesRequired = requiredCalories,

                minProteinRequired = Math.Round(((NutritionalConstants.minPercentCaloriesFromProteins / 100.0) * requiredCalories) / (NutritionalConstants.proteinToCalories), 2),
                maxProteinRequired = Math.Round(((NutritionalConstants.maxPercentCaloriesFromProteins / 100.0) * requiredCalories) / (NutritionalConstants.proteinToCalories), 2),

                minCarbsRequired = Math.Round(((NutritionalConstants.minPercentCaloriesFromCarbs / 100.0) * requiredCalories) / (NutritionalConstants.carbsToCalories),2),
                maxCarbsRequired = Math.Round(((NutritionalConstants.maxPercentCaloriesFromCarbs / 100.0) * requiredCalories) / (NutritionalConstants.carbsToCalories),2),

                minFatRequired = Math.Round(((NutritionalConstants.minPercentCaloriesFromFats / 100.0) * requiredCalories) / (NutritionalConstants.fatToCalories),2),
                maxFatRequired = Math.Round(((NutritionalConstants.maxPercentCaloriesFromFats / 100.0) * requiredCalories) / (NutritionalConstants.fatToCalories), 2),




            };


            return returnedResponse.CorrectResponse(nutritionCalculatorResponseModel);



        }
    }
}
