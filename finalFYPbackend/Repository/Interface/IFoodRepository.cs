using finalFYPbackend.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace finalFYPbackend.Repository.Interface
{
    public interface IFoodRepository
    {
        public Task<Food> createFood(Food food);
        public Task<Food> updateFood(int foodId, string imageUrl);
        public Task<IEnumerable<Food>> getFoods();
    }
}
