using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Model.NutritionModels;
using finalFYPbackend.Responses;
using System.Threading.Tasks;

namespace finalFYPbackend.Services.Interface
{
    public interface INutritionServices
    {
        public Task<ApiResponse> NutritionCalculator(NutritionCalculatorRequestModel model);
        public NutritionCalculatorResponseModel calculateNutrientsFromCalories(double calories);
    }
}
