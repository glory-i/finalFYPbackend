using finalFYPbackend.Authentication;
using finalFYPbackend.Model;
using finalFYPbackend.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finalFYPbackend.Repository.Implementation
{
    public class FoodRepository : IFoodRepository
    {
        private readonly ApplicationDbContext _context;
        public FoodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Food> createFood(Food food)
        {
            try
            {

                food.flutterImageUrl = getFlutterImageFormat(getImageId(food.imageUrl));

                var newFood = await _context.Foods.AddAsync(food);
                await _context.SaveChangesAsync();
                return food;
            }

            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Food>> getFoods()
        {
            try
            {
                var allFoods = await _context.Foods.ToListAsync();
                return allFoods;
            }
            catch(Exception e)
            {
                return null;
            }

        }

        public async Task<Food> updateFood(int foodId, string imageUrl)
        {
            var food = await _context.Foods.Where(f => f.id == foodId).FirstAsync();

            food.imageUrl = imageUrl;
            food.flutterImageUrl = getFlutterImageFormat(getImageId(imageUrl));

            await _context.SaveChangesAsync();
            return food;

        }

        public string getImageId(string imageUrl)
        {
            //return the id of the image passed from the google drive
            var imageUrlList = imageUrl.Split('/').ToList();
            return imageUrlList[5];
        }

        public string getFlutterImageFormat(string imageId)
        {
            //return the url of the image from the google drive in a url that flutter can consume.
            return $"https://drive.google.com/uc?export=view&id={imageId}";

        }

    }
}
