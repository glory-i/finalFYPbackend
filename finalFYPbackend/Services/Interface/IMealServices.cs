using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Model;
using finalFYPbackend.Responses;
using System.Threading.Tasks;

namespace finalFYPbackend.Services.Interface
{
    public interface IMealServices
    {
        public Task<ApiResponse> createMeal(createMealDTO model);
        public Task<ApiResponse> popularBreakfasts();
        public Task<ApiResponse> popularLunches();
        public Task<ApiResponse> popularDinners();
        public Task<ApiResponse> searchMeals(string search);
        public Task<ApiResponse> getMeals(string mealType, string search);
        public Task<ApiResponse> generateMealPlan(string duration, GenerateMealPlanRequestModel model); //duration can either be "Day, Week or Month"
    }

}
