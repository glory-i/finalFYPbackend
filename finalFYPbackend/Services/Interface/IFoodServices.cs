using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Model;
using finalFYPbackend.Responses;
using System.Threading.Tasks;

namespace finalFYPbackend.Services.Interface
{
    public interface IFoodServices
    {
        public Task<ApiResponse> createFood(createFoodDTO model);
        public Task<ApiResponse> updateFood(int foodId, string imageUrl);
        public Task<ApiResponse> createHephzibahMeals(); //this method is to make as many meals possible from the foods served in hephzibah restaurant.
        public Task<ApiResponse> createSharonMeals(); //this method is to make as many meals possible from the foods served in sharon restaurant.
        public Task<ApiResponse> createOliveYardMeals(); //this method is to make as many meals possible from the foods served in olive yard restaurant.(mainly swallow)
        public Task<ApiResponse> createBashanMeals(); //this method is to make as many meals possible from the foods served in bashan restaurant.
        public Task<ApiResponse> createWaakyeAndSpagMeals(); //this method is to make as many meals possible from the foods served from waakye ad spag restaurant (opposite caf).

        public Task<ApiResponse> createEwaGMeals(); //this method is to make as many ewa g meals as possible.
        public Task<ApiResponse> createBreadandEggMeals(); //this method is to make as many ewa g meals as possible.

    }
}
