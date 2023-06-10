using finalFYPbackend.Authentication;
using finalFYPbackend.Model;
using finalFYPbackend.Repository.Interface;
using finalFYPbackend.Responses;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using finalFYPbackend.Responses.Enums;
using System.Collections.Generic;

namespace finalFYPbackend.Repository.Implementation
{
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _context;
        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Meal> createMeal(Meal meal)
        {
            try
            {
                var newMeal = await _context.Meals.AddAsync(meal);
                await _context.SaveChangesAsync();
                return meal;
            }

            catch (Exception e)
            {
                return null;
            }


        }

        public async Task<List<Meal>> getBreakfasts()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> breakfasts = new List<Meal>();

            try
            {
                breakfasts = await _context.Meals.Where(m => m.TypeOfMeal == FoodTypeEnum.Breakfast.GetEnumDescription()).ToListAsync();
                
                //this is strictly for testing purposes
                /*if(breakfasts.Count == 0)
                {
                    //breakfasts.Add(new Meal() { Name = "Bread and Akara", Description = "Fried bean cakes with cornmeal porridge", TypeOfMeal = "Breakfast", Producer = "Street Vendor", Cost = 500, Calories = 400, Protein = 20, Carbs = 50, Fat = 15 });
                    //breakfasts.Add(new Meal() { Name = "Ewa Agoyin and Agege Bread", Description = "Stewed black-eyed peas with soft bread", TypeOfMeal = "Breakfast", Producer = "Street Vendor", Cost = 1.50, Calories = 550, Protein = 20, Carbs = 60, Fat = 20 });

                    List<Meal> breakfasts2 = new List<Meal>()
                    {
                        new Meal { Name = "Chicken Breast", Calories = 265, Protein = 31, Carbs = 300, Cost = 560, Fat= 290, TypeOfMeal = "Breakfast" },
                        new Meal { Name = "Brown Rice", Calories = 215, Protein = 5, Carbs = 350, Cost = 660, Fat= 190,TypeOfMeal = "Breakfast"  },
                        new Meal { Name = "Broccoli", Calories = 55, Protein = 5, Carbs = 360, Cost = 460, Fat= 120, TypeOfMeal = "Breakfast"},
                        new Meal { Name = "Bread", Calories = 75, Protein = 4 ,  Carbs = 400, Cost = 160, Fat= 200,TypeOfMeal = "Breakfast"},
                        new Meal { Name = "Yammarita", Calories = 85, Protein = 6,  Carbs = 210, Cost = 960, Fat= 190, TypeOfMeal = "Breakfast" },
                        new Meal { Name = "Rice and egg", Calories = 95, Protein = 8,  Carbs = 900, Cost = 500, Fat= 200,TypeOfMeal = "Breakfast" },

                        // Add more food items with their nutritional information as needed
                    };
                    return breakfasts2;

                }*/

                return (breakfasts);
            }

            catch(Exception e)
            {
                return null;
            }

        }

        public async Task<List<Meal>> getDinners()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> dinners = new List<Meal>();

            try
            {
                dinners = await _context.Meals.Where(m => m.TypeOfMeal == FoodTypeEnum.Dinner.GetEnumDescription()).ToListAsync();
                
                //this is strictly for testing purposes
                /*if (dinners.Count == 0)
                {
                    List<Meal> dinners2 = new List<Meal>()
                    {
                        new Meal { Name = "Indomie", Calories = 215, Protein = 30, TypeOfMeal = "Dinner",  Carbs = 310, Cost = 500, Fat= 200, },
                        new Meal { Name = "Bole", Calories = 295, Protein = 10,TypeOfMeal = "Dinner",  Carbs = 390, Cost = 2000, Fat= 210, },
                        new Meal { Name = "Suya", Calories = 355, Protein = 15, TypeOfMeal = "Dinner",  Carbs = 400, Cost = 1500, Fat= 200,},
                        new Meal { Name = "J. Spag", Calories = 95, Protein = 9 , TypeOfMeal = "Dinner",  Carbs = 310, Cost = 960, Fat= 280,},
                        new Meal { Name = "Fried rice", Calories = 195, Protein = 6 ,TypeOfMeal = "Dinner",  Carbs = 301, Cost = 1000, Fat= 290, },
                        new Meal { Name = "Nwake rice", Calories = 295, Protein = 8, TypeOfMeal = "Dinner", Carbs = 390, Cost = 860, Fat= 197, },

                        // Add more food items with their nutritional information as needed
                    };
                    return dinners2;

                }*/

                return (dinners);
            }

            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Meal>> getLunches()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> lunches = new List<Meal>();

            try
            {
                lunches = await _context.Meals.Where(m => m.TypeOfMeal == FoodTypeEnum.Lunch.GetEnumDescription()).ToListAsync();
                //if (lunches.Count == 0)
                //{
                //    //lunches.Add(new Meal() { Name = "Rice", Description = "Stewed black-eyed peas with soft bread", TypeOfMeal = "Lunch", Producer = "Street Vendor", Cost = 1.50, Calories = 550, Protein = 20, Carbs = 60, Fat = 20 });
                //    //this is strictly for testing purposes
                //    List<Meal> lunches2 = new List<Meal>()
                //    {
                //        new Meal { Name = "Spaghetti", Calories = 450, Protein = 29, TypeOfMeal = "Lunch",  Carbs = 370, Cost = 890, Fat= 90, },
                //        new Meal { Name = "Rice and Beans", Calories = 300, Protein = 10,TypeOfMeal = "Lunch",  Carbs = 410, Cost = 1760, Fat= 299,  },
                //        new Meal { Name = "Plntain and stew", Calories = 345, Protein = 9, TypeOfMeal = "Lunch",  Carbs = 390, Cost = 1560, Fat= 890,},
                //        new Meal { Name = "Fried yam", Calories = 200, Protein = 19 , TypeOfMeal = "Lunch",  Carbs = 390, Cost = 910, Fat= 140,},
                //        new Meal { Name = "Spaghetti", Calories = 185, Protein = 4 ,TypeOfMeal = "Lunch", Carbs = 280, Cost = 790, Fat= 210, },
                //        new Meal { Name = "Waffles", Calories = 195, Protein = 8, TypeOfMeal = "Lunch",  Carbs = 400, Cost = 960, Fat= 300, },

                //        // Add more food items with their nutritional information as needed
                //    };
                //    return lunches2;
                //}

                return (lunches);
            }

            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ApiResponse> getMeals(string mealType, string search)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            try
            {
                var query = _context.Meals.AsQueryable();
                if (!String.IsNullOrEmpty(mealType))
                {
                    query = query.Where(m => m.TypeOfMeal.Contains(mealType)).Take(5);
                }

                if (!String.IsNullOrEmpty(search))
                {
                    query = query.Where(m => ((m.TypeOfMeal + m.Name + m.Description + m.Producer).Contains(search)));
                }

                var listOfMeals = await query.ToListAsync();

                if (listOfMeals == null)
                {
                    return returnedResponse.ErrorResponse("Unable to retrieve meals", null);
                }

                return returnedResponse.CorrectResponse(listOfMeals);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }


        }

        public async Task<ApiResponse> popularBreakfast()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> newPopularBreakfasts = new List<Meal>();
            try
            {

                Random random = new Random();
                var popularBreakfasts = await _context.Meals.Where(f => f.TypeOfMeal.Contains(FoodTypeEnum.Breakfast.GetEnumDescription())).ToListAsync();

                for (int i = 0; i < 5; i++)
                {
                    var popularBreakfast = popularBreakfasts[random.Next(popularBreakfasts.Count)];
                    newPopularBreakfasts.Add(popularBreakfast);

                    popularBreakfasts.RemoveAll(m => m.Name.Contains(popularBreakfast.Name));
                }


                if (popularBreakfasts == null)
                {
                    returnedResponse.ErrorResponse("Could not retrieve breakfast", null);
                }
                return returnedResponse.CorrectResponse(newPopularBreakfasts);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }


        }

        public async Task<ApiResponse> popularDinner()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> newPopularDinners = new List<Meal>();
            try
            {

                Random random = new Random();
                var popularDinners = await _context.Meals.Where(f => f.TypeOfMeal.Contains(FoodTypeEnum.Dinner.GetEnumDescription())).ToListAsync();

                for (int i = 0; i < 5; i++)
                {
                    var popularDinner = popularDinners[random.Next(popularDinners.Count)];
                    newPopularDinners.Add(popularDinner);

                    popularDinners.RemoveAll(m => m.Name.Contains(popularDinner.Name));
                }


                if (popularDinners == null)
                {
                    returnedResponse.ErrorResponse("Could not retrieve dinner", null);
                }
                return returnedResponse.CorrectResponse(newPopularDinners);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }

        }

        public async Task<ApiResponse> popularLunch()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> newPopularLunches = new List<Meal>();
            try
            {
                // var popularLunches = await _context.Meals.Where(f => f.TypeOfMeal.Contains(FoodTypeEnum.Lunch.GetEnumDescription())).Take(5).ToListAsync();

                Random random = new Random();
                var popularLunches = await _context.Meals.Where(f => f.TypeOfMeal.Contains(FoodTypeEnum.Lunch.GetEnumDescription())).ToListAsync();

                for(int i = 0; i < 5; i++)
                {
                    var popularLunch = popularLunches[random.Next(popularLunches.Count)];
                    newPopularLunches.Add(popularLunch);

                    popularLunches.RemoveAll(m => m.Name.Contains(popularLunch.Name));
                }


                if (popularLunches == null)
                {
                    returnedResponse.ErrorResponse("Could not retrieve lunches", null);
                }
                return returnedResponse.CorrectResponse(newPopularLunches);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }
        }

        public async Task<ApiResponse> searchMeals(string search)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            try
            {
                var searchedMeals = await _context.Meals.Where(m => (m.TypeOfMeal + m.Name + m.Description + m.Producer).Contains(search)).ToListAsync();
                if (searchedMeals == null)
                {
                    return returnedResponse.ErrorResponse("Could not find any meal at this time", null);
                }

                return returnedResponse.CorrectResponse(searchedMeals);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }

        }
    }
}
