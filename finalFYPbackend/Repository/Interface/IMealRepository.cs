using finalFYPbackend.Model;
using finalFYPbackend.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace finalFYPbackend.Repository.Interface
{
    public interface IMealRepository
    {
        public Task<Meal> createMeal(Meal meal);
        public Task<ApiResponse> popularBreakfast();  //get 5 most popular breakfasts
        public Task<ApiResponse> popularLunch(); //get 5 most popular lunches
        public Task<ApiResponse> popularDinner(); //get 5 most popular dinners

        public Task<List<Meal>> getBreakfasts();
        public Task<List<Meal>> getLunches();
        public Task<List<Meal>> getDinners();


        public Task<ApiResponse> searchMeals(string search); //get meals that contain the searched string

        public Task<ApiResponse> getMeals(string mealType, string search); //one end point that does the same as the other 4 in 1. to test myself

    }
}
