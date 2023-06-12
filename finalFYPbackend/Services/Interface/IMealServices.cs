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
        public Task<ApiResponse> generateMealPlan(string username, string duration, GenerateMealPlanRequestModel model); //duration can either be "Day, Week or Month"
        public Task<ApiResponse> regenerateMealPlan(int index, string username, string duration, GenerateMealPlanRequestModel model); //duration can either only be day
        public ApiResponse getBudgetForDay(); //duration can either only be day
        public ApiResponse getBudgetForWeek(); //duration can either only be day
        public ApiResponse getBudgetForMonth(); //duration can either only be day
        public Task<ApiResponse> updateMealsValues(); //every meal should have 2 dp values for double values
    }

}
