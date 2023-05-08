using AutoMapper;
using finalFYPbackend.Authentication;
using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Model;
using finalFYPbackend.Repository.Implementation;
using finalFYPbackend.Repository.Interface;
using finalFYPbackend.Responses;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finalFYPbackend.Services.Implementation
{
    public class FoodServices : IFoodServices
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMealRepository _mealRepository;
        public FoodServices(IFoodRepository foodRepository, IMealRepository mealRepository)
        {
            _foodRepository = foodRepository;
            _mealRepository = mealRepository;
        }

        

        public async Task<ApiResponse> createFood(createFoodDTO model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
            var food = mapper.Map<Food>(model);

            food = await _foodRepository.createFood(food);
            
            if(food == null)
            {
                return returnedResponse.ErrorResponse("Could not create food", null);
            }

            return returnedResponse.CorrectResponse(food);

            //MAY STILL USE THIS WHEN GENERATING THE PICTURES
            /*if (model.imageUrl != null)
            {
                var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
                var food = mapper.Map<Food>(model);
                food.flutterImageUrl = getFlutterImageFormat(getImageId(model.imageUrl));

                food = await _mealRepository.createMeal(food);
                if (food == null)
                {
                    return returnedResponse.ErrorResponse("Could not create food", null);
                }

                return returnedResponse.CorrectResponse(food);

            }

            else
            {
                return returnedResponse.ErrorResponse("No Image For this meal was provided.", null);
            }
            */

        }

        
        //all that is left here is to add the pictures AND save the meals created to the database.
        public async Task<ApiResponse> createHephzibahMeals()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            List<Meal> hephzibahmeals = new List<Meal>();

            //take all the meals from hephzibah 
            var allFoods = await _foodRepository.getFoods();

            var hephzibahFoods = allFoods.Where(f => f.Producer.Contains(RestaurantEnum.HephzibahRestaurant.GetEnumDescription())).ToList();

            var primaryFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Primary.GetEnumDescription())).ToList();

            var secondaryFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Secondary.GetEnumDescription())).ToList();

            var proteinFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Protein.GetEnumDescription())).ToList();
            
            var tertiaryFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Tertiary.GetEnumDescription())).ToList();
            
            var stewFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Stew.GetEnumDescription())).ToList();
            
            var miscFoods = hephzibahFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Miscellaneous.GetEnumDescription())).ToList();



           
            //combine each primary food with a secondary food.

            foreach(var primaryFood in primaryFoods)
            {

                foreach(var secondaryFood in secondaryFoods)
                {
                    if (primaryFood.Name == secondaryFood.Name) continue;
                    
                    foreach(var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{primaryFood.Name} and {secondaryFood.Name} and {proteinFood.Name}",
                            Description = $"{primaryFood.Description} and {secondaryFood.Description} and {proteinFood.Description}",
                            Calories = primaryFood.Calories + secondaryFood.Calories + proteinFood.Calories,
                            Cost = primaryFood.Cost + secondaryFood.Cost + proteinFood.Cost,
                            Protein = primaryFood.Protein + secondaryFood.Protein + proteinFood.Protein,
                            Carbs = primaryFood.Carbs + secondaryFood.Carbs + proteinFood.Carbs,
                            Fat = primaryFood.Fat + secondaryFood.Fat + proteinFood.Fat,
                            TypeOfMeal = primaryFood.TypeOfMeal,
                            Producer = primaryFood.Producer,
                            imageUrl = primaryFood.imageUrl,
                            flutterImageUrl = primaryFood.flutterImageUrl
                           
                        };

                        hephzibahmeals.Add(meal);
                    }
                }
            }

            //combine each primary food with a protein.
            foreach (var primaryFood in primaryFoods)
            {
                foreach(var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{primaryFood.Name} and  {proteinFood.Name}",
                        Description = $"{primaryFood.Description} and {proteinFood.Description}",
                        Calories = primaryFood.Calories  + proteinFood.Calories,
                        Cost = primaryFood.Cost  + proteinFood.Cost,
                        Protein = primaryFood.Protein + proteinFood.Protein,
                        Carbs = primaryFood.Carbs +  proteinFood.Carbs,
                        Fat = primaryFood.Fat + proteinFood.Fat,
                        TypeOfMeal = primaryFood.TypeOfMeal,
                        Producer = primaryFood.Producer,
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl
                    };

                    hephzibahmeals.Add(meal);
                }
                
            }


            //combine each tertiary food with a stew
            foreach(var tertiaryFood in tertiaryFoods)
            {
                foreach(var stew in stewFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{tertiaryFood.Name} and {stew.Name}",
                        Description = $"{tertiaryFood.Description}  and {stew.Description}",
                        Calories = tertiaryFood.Calories  + stew.Calories,
                        Cost = tertiaryFood.Cost + stew.Cost,
                        Protein = tertiaryFood.Protein +  + stew.Protein,
                        Carbs = tertiaryFood.Carbs + stew.Carbs,
                        Fat = tertiaryFood.Fat + stew.Fat,
                        TypeOfMeal = tertiaryFood.TypeOfMeal,
                        Producer = tertiaryFood.Producer,
                        imageUrl = tertiaryFood.imageUrl,
                        flutterImageUrl = tertiaryFood.flutterImageUrl,
                    };

                    hephzibahmeals.Add(meal);
                }
            }


            //combine each miscellaneous food with a protein.
            foreach (var miscFood in miscFoods)
            {
                foreach (var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{miscFood.Name} and  {proteinFood.Name}",
                        Description = $"{miscFood.Description} and {proteinFood.Description}",
                        Calories = miscFood.Calories + proteinFood.Calories,
                        Cost = miscFood.Cost + proteinFood.Cost,
                        Protein = miscFood.Protein + proteinFood.Protein,
                        Carbs = miscFood.Carbs + proteinFood.Carbs,
                        Fat = miscFood.Fat + proteinFood.Fat,
                        TypeOfMeal = miscFood.TypeOfMeal,
                        Producer = miscFood.Producer,
                        imageUrl = miscFood.imageUrl,
                        flutterImageUrl = miscFood.flutterImageUrl
                    };

                    hephzibahmeals.Add(meal);
                }

            }

            //send everything to the database.
            foreach(var meal in hephzibahmeals)
            {
                var hephzibahMeal = await _mealRepository.createMeal(meal);
            }

            return returnedResponse.CorrectResponse(hephzibahmeals);


        }


        //all that is left here is to add the pictures AND save the meals created to the database.
        public async Task<ApiResponse> createOliveYardMeals()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            List<Meal> oliveYardMeals = new List<Meal>();

            //take all the meals from oliveyard 
            var allFoods = await _foodRepository.getFoods();

            var oliveYardFoods = allFoods.Where(f => f.Producer.Contains(RestaurantEnum.OliveYardRestaurant.GetEnumDescription())).ToList();

            var proteinFoods = oliveYardFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Protein.GetEnumDescription())).ToList();

            var swallowFoods = oliveYardFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Swallow.GetEnumDescription())).ToList();

            var soupFoods = oliveYardFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Soup.GetEnumDescription())).ToList();



            //combine each swallow food with a soup and a protein


            foreach (var swallowFood in swallowFoods)
            {

                foreach (var soupFood in soupFoods)
                {
                    if (swallowFood.Name == soupFood.Name) continue;

                    foreach (var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{swallowFood.Name} and {soupFood.Name} and {proteinFood.Name}",
                            Description = $"{swallowFood.Description} and {soupFood.Description} and {proteinFood.Description}",
                            Calories = swallowFood.Calories + soupFood.Calories + proteinFood.Calories,
                            Cost = swallowFood.Cost + soupFood.Cost + proteinFood.Cost,
                            Protein = swallowFood.Protein + soupFood.Protein + proteinFood.Protein,
                            Carbs = swallowFood.Carbs + soupFood.Carbs + proteinFood.Carbs,
                            Fat = swallowFood.Fat + soupFood.Fat + proteinFood.Fat,
                            TypeOfMeal = swallowFood.TypeOfMeal,
                            Producer = swallowFood.Producer,
                            imageUrl = swallowFood.imageUrl,
                            flutterImageUrl = swallowFood.flutterImageUrl

                        };

                        oliveYardMeals.Add(meal);
                    }
                }
            }

            foreach(var meal in oliveYardMeals)
            {
                var oliveYardMeal = await _mealRepository.createMeal(meal);
            }

            return returnedResponse.CorrectResponse(oliveYardMeals);

        }


        //all that is left here is to add the pictures AND save the meals created to the database.
        public async Task<ApiResponse> createSharonMeals()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            List<Meal> sharonMeals = new List<Meal>();

            //take all the meals from sharon 
            var allFoods = await _foodRepository.getFoods();

            var sharonFoods = allFoods.Where(f => f.Producer.Contains(RestaurantEnum.SharonRestaurant.GetEnumDescription())).ToList();

            var primaryFoods = sharonFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Primary.GetEnumDescription())).ToList();

            var secondaryFoods = sharonFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Secondary.GetEnumDescription())).ToList();

            var proteinFoods = sharonFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Protein.GetEnumDescription())).ToList();

            var swallowFoods = sharonFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Swallow.GetEnumDescription())).ToList();

            var soupFoods = sharonFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Soup.GetEnumDescription())).ToList();





            //combine each primary food with a secondary food and a protein.

            foreach (var primaryFood in primaryFoods)
            {

                foreach (var secondaryFood in secondaryFoods)
                {
                    if (primaryFood.Name == secondaryFood.Name) continue;

                    foreach (var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{primaryFood.Name} and {secondaryFood.Name} and {proteinFood.Name}",
                            Description = $"{primaryFood.Description} and {secondaryFood.Description} and {proteinFood.Description}",
                            Calories = primaryFood.Calories + secondaryFood.Calories + proteinFood.Calories,
                            Cost = primaryFood.Cost + secondaryFood.Cost + proteinFood.Cost,
                            Protein = primaryFood.Protein + secondaryFood.Protein + proteinFood.Protein,
                            Carbs = primaryFood.Carbs + secondaryFood.Carbs + proteinFood.Carbs,
                            Fat = primaryFood.Fat + secondaryFood.Fat + proteinFood.Fat,
                            TypeOfMeal = primaryFood.TypeOfMeal,
                            Producer = primaryFood.Producer,
                            imageUrl = primaryFood.imageUrl,
                            flutterImageUrl = primaryFood.flutterImageUrl

                        };

                        sharonMeals.Add(meal);
                    }
                }
            }

            //combine each primary food with a protein.
            foreach (var primaryFood in primaryFoods)
            {
                foreach (var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{primaryFood.Name} and  {proteinFood.Name}",
                        Description = $"{primaryFood.Description} and {proteinFood.Description}",
                        Calories = primaryFood.Calories + proteinFood.Calories,
                        Cost = primaryFood.Cost + proteinFood.Cost,
                        Protein = primaryFood.Protein + proteinFood.Protein,
                        Carbs = primaryFood.Carbs + proteinFood.Carbs,
                        Fat = primaryFood.Fat + proteinFood.Fat,
                        TypeOfMeal = primaryFood.TypeOfMeal,
                        Producer = primaryFood.Producer,
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl,
                        
                    };

                    sharonMeals.Add(meal);
                }

            }


            //combine each swallow food with a soup and a protein


            foreach (var swallowFood in swallowFoods)
            {

                foreach (var soupFood in soupFoods)
                {
                    if (swallowFood.Name == soupFood.Name) continue;

                    foreach (var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{swallowFood.Name} and {soupFood.Name} and {proteinFood.Name}",
                            Description = $"{swallowFood.Description} and {soupFood.Description} and {proteinFood.Description}",
                            Calories = swallowFood.Calories + soupFood.Calories + proteinFood.Calories,
                            Cost = swallowFood.Cost + soupFood.Cost + proteinFood.Cost,
                            Protein = swallowFood.Protein + soupFood.Protein + proteinFood.Protein,
                            Carbs = swallowFood.Carbs + soupFood.Carbs + proteinFood.Carbs,
                            Fat = swallowFood.Fat + soupFood.Fat + proteinFood.Fat,
                            TypeOfMeal = swallowFood.TypeOfMeal,
                            Producer = swallowFood.Producer,
                            imageUrl = swallowFood.imageUrl,
                            flutterImageUrl = swallowFood.flutterImageUrl
                        };

                        sharonMeals.Add(meal);
                    }
                }
            }


            foreach(var meal in sharonMeals)
            {
                var sharonMeal = await _mealRepository.createMeal(meal);
            }
            return returnedResponse.CorrectResponse(sharonMeals);


        }


        public async Task<ApiResponse> createBashanMeals()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            List<Meal> bashanMeals = new List<Meal>();

            //take all the meals from bashan 
            var allFoods = await _foodRepository.getFoods();

            var bashanFoods = allFoods.Where(f => f.Producer.Contains(RestaurantEnum.BashanRestaurant.GetEnumDescription())).ToList();

            var primaryFoods = bashanFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Primary.GetEnumDescription())).ToList();

            var secondaryFoods = bashanFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Secondary.GetEnumDescription())).ToList();

            var proteinFoods = bashanFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Protein.GetEnumDescription())).ToList();


            var miscFoods = bashanFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Miscellaneous.GetEnumDescription())).ToList();




            //combine each primary food with a secondary food.

            foreach (var primaryFood in primaryFoods)
            {

                foreach (var secondaryFood in secondaryFoods)
                {
                    if (primaryFood.Name == secondaryFood.Name) continue;

                    foreach (var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{primaryFood.Name} and {secondaryFood.Name} and {proteinFood.Name}",
                            Description = $"{primaryFood.Description} and {secondaryFood.Description} and {proteinFood.Description}",
                            Calories = primaryFood.Calories + secondaryFood.Calories + proteinFood.Calories,
                            Cost = primaryFood.Cost + secondaryFood.Cost + proteinFood.Cost,
                            Protein = primaryFood.Protein + secondaryFood.Protein + proteinFood.Protein,
                            Carbs = primaryFood.Carbs + secondaryFood.Carbs + proteinFood.Carbs,
                            Fat = primaryFood.Fat + secondaryFood.Fat + proteinFood.Fat,
                            TypeOfMeal = primaryFood.TypeOfMeal,
                            Producer = primaryFood.Producer,
                            imageUrl = primaryFood.imageUrl,
                            flutterImageUrl = primaryFood.flutterImageUrl

                        };

                        bashanMeals.Add(meal);
                    }
                }
            }


            //combine each primary food with a protein.
            foreach (var primaryFood in primaryFoods)
            {
                foreach (var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{primaryFood.Name} and  {proteinFood.Name}",
                        Description = $"{primaryFood.Description} and {proteinFood.Description}",
                        Calories = primaryFood.Calories + proteinFood.Calories,
                        Cost = primaryFood.Cost + proteinFood.Cost,
                        Protein = primaryFood.Protein + proteinFood.Protein,
                        Carbs = primaryFood.Carbs + proteinFood.Carbs,
                        Fat = primaryFood.Fat + proteinFood.Fat,
                        TypeOfMeal = primaryFood.TypeOfMeal,
                        Producer = primaryFood.Producer,
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl

                    };

                    bashanMeals.Add(meal);
                }

            }


            //combine each miscellaneous food with a protein.
            foreach (var miscFood in miscFoods)
            {
                foreach (var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{miscFood.Name} and  {proteinFood.Name}",
                        Description = $"{miscFood.Description} and {proteinFood.Description}",
                        Calories = miscFood.Calories + proteinFood.Calories,
                        Cost = miscFood.Cost + proteinFood.Cost,
                        Protein = miscFood.Protein + proteinFood.Protein,
                        Carbs = miscFood.Carbs + proteinFood.Carbs,
                        Fat = miscFood.Fat + proteinFood.Fat,
                        TypeOfMeal = miscFood.TypeOfMeal,
                        Producer = miscFood.Producer,
                        imageUrl = miscFood.imageUrl,
                        flutterImageUrl = miscFood.flutterImageUrl

                    };

                    bashanMeals.Add(meal);
                }

            }

            foreach(var meal in bashanMeals)
            {
                var bashanMeal = await _mealRepository.createMeal(meal);
            }

            return returnedResponse.CorrectResponse(bashanMeals);

        }

        public async Task<ApiResponse> createWaakyeAndSpagMeals()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            List<Meal> meals = new List<Meal>();

            //take all the meals from hephzibah 
            var allFoods = await _foodRepository.getFoods();

            allFoods = allFoods.Where(f => f.Producer.Contains(RestaurantEnum.WaakyeandSpagRestaurant.GetEnumDescription())).ToList();

            var primaryFoods = allFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Primary.GetEnumDescription())).ToList();

            var secondaryFoods = allFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Secondary.GetEnumDescription())).ToList();

            var proteinFoods = allFoods.Where(f => f.CategoryOfFood.Contains(FoodCategoryEnum.Protein.GetEnumDescription())).ToList();

            


            //combine each primary food with a secondary food and a protein.

            foreach (var primaryFood in primaryFoods)
            {

                foreach (var secondaryFood in secondaryFoods)
                {
                    if (primaryFood.Name == secondaryFood.Name) continue;

                    foreach (var proteinFood in proteinFoods)
                    {
                        Meal meal = new Meal
                        {
                            Name = $"{primaryFood.Name} and {secondaryFood.Name} and {proteinFood.Name}",
                            Description = $"{primaryFood.Description} and {secondaryFood.Description} and {proteinFood.Description}",
                            Calories = primaryFood.Calories + secondaryFood.Calories + proteinFood.Calories,
                            Cost = primaryFood.Cost + secondaryFood.Cost + proteinFood.Cost,
                            Protein = primaryFood.Protein + secondaryFood.Protein + proteinFood.Protein,
                            Carbs = primaryFood.Carbs + secondaryFood.Carbs + proteinFood.Carbs,
                            Fat = primaryFood.Fat + secondaryFood.Fat + proteinFood.Fat,
                            TypeOfMeal = primaryFood.TypeOfMeal,
                            Producer = primaryFood.Producer,
                            imageUrl = primaryFood.imageUrl,
                            flutterImageUrl = primaryFood.flutterImageUrl

                        };

                        meals.Add(meal);
                    }
                }
            }

            //combine each primary food with a protein.
            foreach (var primaryFood in primaryFoods)
            {
                foreach (var proteinFood in proteinFoods)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{primaryFood.Name} and  {proteinFood.Name}",
                        Description = $"{primaryFood.Description} and {proteinFood.Description}",
                        Calories = primaryFood.Calories + proteinFood.Calories,
                        Cost = primaryFood.Cost + proteinFood.Cost,
                        Protein = primaryFood.Protein + proteinFood.Protein,
                        Carbs = primaryFood.Carbs + proteinFood.Carbs,
                        Fat = primaryFood.Fat + proteinFood.Fat,
                        TypeOfMeal = primaryFood.TypeOfMeal,
                        Producer = primaryFood.Producer,
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl

                    };

                    meals.Add(meal);
                }

            }

            foreach(var meal in meals)
            {
                var waakyeAndSpagMeals = await _mealRepository.createMeal(meal);
            }
            return returnedResponse.CorrectResponse(meals);

        }

        public async Task<ApiResponse> updateFood(int foodId, string imageUrl)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            var food = await _foodRepository.updateFood(foodId, imageUrl);
            return returnedResponse.CorrectResponse(food);

        }

        public async Task<ApiResponse> createEwaGMeals()
        {
            var allFoods = await _foodRepository.getFoods();
            var primaryFood = allFoods.Where(f => f.CategoryOfFood == FoodCategoryEnum.Primary.GetEnumDescription() && f.Producer == RestaurantEnum.EwaGStand.GetEnumDescription()).First();
            var proteinFood = allFoods.Where(f => f.CategoryOfFood == FoodCategoryEnum.Protein.GetEnumDescription() && f.Producer == RestaurantEnum.EwaGStand.GetEnumDescription()).First();


            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> meals = new List<Meal>();

            if(primaryFood.Name == "Bread roll")
            {
                for (int i = 1; i < 7; i++)
                {
                    Meal meal = new Meal
                    {
                        Name = $"{primaryFood.Name} and {proteinFood.Name}",
                        Calories = (i * primaryFood.Calories) + (i * proteinFood.Calories),
                        Cost = (i * primaryFood.Cost) + (i * proteinFood.Cost),
                        Protein = (i * primaryFood.Protein) + (i * proteinFood.Protein),
                        Carbs = (i * primaryFood.Carbs) + (i * proteinFood.Carbs),
                        Fat = (i * primaryFood.Fat) + (i * proteinFood.Fat),
                        Producer = primaryFood.Producer,
                        Description = $"{digitToNumber(i)} {primaryFood.Name}s and {digitToNumber(i)} spoons of {proteinFood.Name}",
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl,
                        TypeOfMeal = primaryFood.TypeOfMeal,
                                               

                    };
                    meals.Add(meal);
                }
            }

            foreach(var meal in meals)
            {
                var ewagmeal = await _mealRepository.createMeal(meal);
            }
            return await Task.FromResult(returnedResponse.CorrectResponse(meals));

            
        }

        public string digitToNumber(int number)
        {
            string n = "";
            switch (number)
            {
                case 1:
                    n =  "one"; break;
                case 2:
                    n= "two"; break;
                case 3:
                    n= "three"; break;
                case 4:
                    n = "four"; break;
                case 5:
                    n = "five"; break;
                case 6:
                    n = "six"; break;
                case 7:
                    n = "seven"; break;
                case 8:
                    n = "eight"; break;
                case 9:
                    n = "nine"; break;

            }
            return n;
        }

        public async Task<ApiResponse> createBreadandEggMeals()
        {
            //throw new System.NotImplementedException();

            var allFoods = await _foodRepository.getFoods();
            var primaryFoods = allFoods.Where(f => f.CategoryOfFood == FoodCategoryEnum.Primary.GetEnumDescription() && f.Producer == RestaurantEnum.BreadandEggStand.GetEnumDescription());
            var proteinFood = allFoods.Where(f => f.CategoryOfFood == FoodCategoryEnum.Protein.GetEnumDescription() && f.Producer == RestaurantEnum.BreadandEggStand.GetEnumDescription()).First();


            ReturnedResponse returnedResponse = new ReturnedResponse();
            List<Meal> meals = new List<Meal>();
            foreach(var primaryFood in primaryFoods)
            {
                for (int i = 1; i < 4; i++)
                {
                    Meal meal = new Meal
                    {
                        Name = primaryFood.Name,
                        Calories = (i * primaryFood.Calories) + (i * proteinFood.Calories),
                        Cost = (i * primaryFood.Cost) + (i * proteinFood.Cost),
                        Protein = (i * primaryFood.Protein) + (i * proteinFood.Protein),
                        Carbs = (i * primaryFood.Carbs) + (i * proteinFood.Carbs),
                        Fat = (i * primaryFood.Fat) + (i * proteinFood.Fat),
                        Producer = primaryFood.Producer,
                        Description = $"{digitToNumber(i)} {primaryFood.Name} and {digitToNumber(i)}  {proteinFood.Name}",
                        imageUrl = primaryFood.imageUrl,
                        flutterImageUrl = primaryFood.flutterImageUrl,
                        TypeOfMeal = primaryFood.TypeOfMeal,

                    };
                    meals.Add(meal);
                }

            }

            foreach(var meal in meals)
            {
                var breadandEggmeal = await _mealRepository.createMeal(meal);
            }

            return returnedResponse.CorrectResponse(meals);
            

        }
    }
}
