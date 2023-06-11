using AutoMapper;
using finalFYPbackend.Authentication;
using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Model;
using finalFYPbackend.Model.NutritionModels;
using finalFYPbackend.Repository.Interface;
using finalFYPbackend.Responses;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Services.Interface;
using finalFYPbackend.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finalFYPbackend.Services.Implementation
{
    public class MealServices : IMealServices
    {
        private readonly IMealRepository _mealRepository;
        private readonly ApplicationDbContext _context;
        public MealServices(IMealRepository mealRepository, ApplicationDbContext context)
        {
            _mealRepository = mealRepository;
            _context = context;
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


        public async Task<ApiResponse> createMeal(createMealDTO model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            if (model.imageUrl != null)
            {
                var mapper = new Mapper(MapperConfig.GetMapperConfiguration());
                var meal = mapper.Map<Meal>(model);
                meal.flutterImageUrl = getFlutterImageFormat(getImageId(model.imageUrl));

                meal = await _mealRepository.createMeal(meal);
                if (meal == null)
                {
                    return returnedResponse.ErrorResponse("Could not create food", null);
                }

                return returnedResponse.CorrectResponse(meal);

            }

            else
            {
                return returnedResponse.ErrorResponse("No Image For this meal was provided.", null);
            }

        }

        public async Task<ApiResponse> popularBreakfasts()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            var popularBreakfastsResponse = await _mealRepository.popularBreakfast();
            if (popularBreakfastsResponse.error == null)
            {
                var popularBreakfasts = popularBreakfastsResponse.data;
                return returnedResponse.CorrectResponse(popularBreakfasts);

            }

            return returnedResponse.ErrorResponse(popularBreakfastsResponse.error.message, null);


        }

        public async Task<ApiResponse> popularLunches()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            var popularLunchesResponse = await _mealRepository.popularLunch();
            if (popularLunchesResponse.error == null)
            {
                var popularLunches = popularLunchesResponse.data;
                return returnedResponse.CorrectResponse(popularLunches);

            }

            return returnedResponse.ErrorResponse(popularLunchesResponse.error.message, null);


        }

        public async Task<ApiResponse> popularDinners()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            var popularDinnersResponse = await _mealRepository.popularDinner();
            if (popularDinnersResponse.error == null)
            {
                var popularDinners = popularDinnersResponse.data;
                return returnedResponse.CorrectResponse(popularDinners);

            }

            return returnedResponse.ErrorResponse(popularDinnersResponse.error.message, null);

        }

        public async Task<ApiResponse> searchMeals(string search)
        {

            ReturnedResponse returnedResponse = new ReturnedResponse();
            var searchMealsResponse = await _mealRepository.searchMeals(search);

            if (searchMealsResponse.error == null)
            {
                var searchedMeals = searchMealsResponse.data;
                return returnedResponse.CorrectResponse(searchedMeals);
            }

            return returnedResponse.ErrorResponse(searchMealsResponse.error.message, null);
        }

        public async Task<ApiResponse> getMeals(string mealType, string search)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            var getMealsResponse = await _mealRepository.getMeals(mealType, search);

            if (getMealsResponse.error == null)
            {
                var meals = getMealsResponse.data;
                return returnedResponse.CorrectResponse(meals);
            }

            return returnedResponse.ErrorResponse(getMealsResponse.error.message, null);
        }

        public async Task<ApiResponse> generateMealPlan(string username, string duration, GenerateMealPlanRequestModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            var user = await _context.Users.Where(u => u.UserName == username).FirstAsync();
            model.calorieRequirements = user.CalorieRequirement;


            try
            {
                int noOfDays = 0;

                if (duration == DurationEnum.Day.GetEnumDescription())
                {
                    noOfDays = 1;
                }

                if (duration == DurationEnum.Week.GetEnumDescription())
                {
                    noOfDays = 7;
                }

                if (duration == DurationEnum.Month.GetEnumDescription())
                {
                    noOfDays = 30;
                }

                //IMPORTANTTTTT. i will need to divide your min and max budget by your number of days. so if it is 60k for 28(or 30) days that will be about 2k per day. VERY IMPORTANT

                model.minBudget = model.minBudget / Convert.ToDouble(noOfDays);
                model.maxBudget = model.maxBudget / Convert.ToDouble(noOfDays);

                //I AM NOT YET SO SURE OF THE ABOVE TWO LINES SHA O.

                MealPlanPopulation population = new MealPlanPopulation
                {
                    mealPlans = new List<MealPlan>(),
                    populationSize = MealPlanConstants.populationSize,
                };

                FinalMealPlan finalMealPlan = new FinalMealPlan { mealPlans = new List<MealPlan>() };

                //this for lopp gets the best meal plan for the day and adds it. in the end it will bring a list of 1 , 7 or 28 meal plans.
                for (int i = 0; i < noOfDays; i++)
                {

                    //this for loop uses genetic algorithm to generate a meal plan for a single day.
                    for (int j = 0; j < MealPlanConstants.noOfGenerations; j++)
                    {

                        //STEP 1 - create initial population
                        population = await createInitialPopulation(MealPlanConstants.populationSize);


                        //STEP 2 - evaluate the fitness of each mealplan in initial population
                        foreach (var mealPlan in population.mealPlans)
                        {
                            var mealPlanFitness = fitnessFunction(mealPlan, model);
                            mealPlan.fitness = mealPlanFitness;
                        }

                        //STEP 3- SELECTION : next is to select parents which will involve roulette wheel 
                        var parentpairs = generateParents(population);


                        //STEP 4- CROSSOVER. : generate offspring by crossing over between pairs of parents
                        //use crossover to create offspring in pairs for the next gen/population.

                        var childrenpairs = generateOffspring(parentpairs, 3, model);

                        //STEP 5- MUTATION, continue from here
                        //mutate each offspring
                        foreach (var childpair in childrenpairs)
                        {
                            await mutation(childpair, MealPlanConstants.mutationRate);

                        }

                        //clear population currently and set new population to new mutated offspring
                        population.mealPlans.Clear();

                        foreach (var childpair in childrenpairs)
                        {
                            population.mealPlans.Add(childpair.Child1);
                            population.mealPlans.Add(childpair.Child2);
                        }


                    }
                    //get the meal plan in the last generation with the highest fitness- that is the final answer. that is the best meal plan FOR THE DAY. add it to final meal plan for the week or month
                    MealPlan bestMealPlan = population.mealPlans.OrderByDescending(f => f.fitness).First();
                    finalMealPlan.mealPlans.Add(bestMealPlan);

                }

                return returnedResponse.CorrectResponse(finalMealPlan);
            }

            catch(Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }



        }

        public async Task<MealPlanPopulation> createInitialPopulation(int populationSize)
        {
            //return a population (initial population) of meal plans
        

            MealPlanPopulation population = new MealPlanPopulation { populationSize = populationSize, mealPlans = new List<MealPlan>(), };
            for (int i = 0; i < populationSize; i++)
            {
                population.mealPlans.Add(await createSingleMealPlan());
            }

            return population;
        }

        public async Task<MealPlan> createSingleMealPlan()
        {
            //this method creates a chromosome - which is a single meal plan (for a day).
            
            //select a random breakfast, lunch and dinner and add them to the list.
            Random random = new Random();

            var breakfasts = (await _mealRepository.getBreakfasts());
            var lunches = (await _mealRepository.getLunches());
            var dinners = (await _mealRepository.getDinners());

            Meal breakfast = breakfasts[random.Next(breakfasts.Count)];
            Meal lunch = lunches[random.Next(lunches.Count)];
            Meal dinner = dinners[random.Next(dinners.Count)];

            
            //create list of meals, sum up the calories, cost, carbs, etc.
            var listOfMeals = new List<Meal> { breakfast, lunch, dinner };

            MealPlan mealPlan = new MealPlan { meals = listOfMeals, };
            mealPlan = calculateMealPlanProperties(mealPlan);

            
            return mealPlan;

        }

        public GenerateMealPlanRequest generateMealPlanRequest(GenerateMealPlanRequestModel model)
        {
            GenerateMealPlanRequest request = new GenerateMealPlanRequest
            {
                calorieRequirements = model.calorieRequirements,
                maxBudget = model.maxBudget,
                minBudget = model.minBudget,
                minProtein = Math.Round(((NutritionalConstants.minPercentCaloriesFromProteins / 100.0) * model.calorieRequirements) / (NutritionalConstants.proteinToCalories), 2),
                maxProtein = Math.Round(((NutritionalConstants.maxPercentCaloriesFromProteins / 100.0) * model.calorieRequirements) / (NutritionalConstants.proteinToCalories), 2),
                minCarbs = Math.Round(((NutritionalConstants.minPercentCaloriesFromCarbs / 100.0) * model.calorieRequirements) / (NutritionalConstants.carbsToCalories),2),
                maxCarbs = Math.Round(((NutritionalConstants.maxPercentCaloriesFromCarbs / 100.0) * model.calorieRequirements) / (NutritionalConstants.carbsToCalories),2),
                minFats = Math.Round(((NutritionalConstants.minPercentCaloriesFromFats / 100.0) * model.calorieRequirements) / (NutritionalConstants.fatToCalories),2),
                maxFats = Math.Round(((NutritionalConstants.maxPercentCaloriesFromFats / 100.0) * model.calorieRequirements) / (NutritionalConstants.fatToCalories),2),

            };

            return request;
        }
        
        //I NEED TO CHANGE THIS FITNESS FUNCTION, IT IS GIVING ME NEGATIVE VALUES WHICH IS NOT IDEAL
        public double fitnessFunction(MealPlan mealPlan, GenerateMealPlanRequestModel model)
        {
            var mealPlanRequest = generateMealPlanRequest(model);

            double totalCalories = mealPlan.meals.Sum(m => m.Calories);
            double totalCost = mealPlan.meals.Sum(m => m.Cost);
            double totalProtein = mealPlan.meals.Sum(m => m.Cost);
            double totalFat = mealPlan.meals.Sum(m => m.Fat);
            double totalCarbs = mealPlan.meals.Sum(m => m.Carbs);

            //NOTE I GOT THE FITNESS FUNCTIONS FROM CHAT GPT, I MAY NEED TO MODIFY IT 

            //fitness on based on how well it meets calories requirements.
            double calorieFitness = 1 - Math.Abs((totalCalories - mealPlanRequest.calorieRequirements) / mealPlanRequest.calorieRequirements);

            //fitness based on how well it meets cost requirements. (upper and lower bound)
            double costFitness = 1 - (Math.Abs(totalCost - ((mealPlanRequest.maxBudget + mealPlanRequest.minBudget) / 2)) / (mealPlanRequest.maxBudget - mealPlanRequest.minBudget));


            /*
            //fitness based on how well it meets protein requirements. (upper and lower bound)
            double proteinFitness = 1 - (Math.Abs(totalProtein - ((mealPlanRequest.maxProtein + mealPlanRequest.minProtein) / 2)) / (mealPlanRequest.maxProtein - mealPlanRequest.minProtein));

            
           //fitness based on how well it meets cost requirements. (upper and lower bound)
            double costFitness = 1 - (Math.Abs(totalCost - ((mealPlanRequest.maxBudget + mealPlanRequest.minBudget) / 2)) / (mealPlanRequest.maxBudget - mealPlanRequest.minBudget));


            //fitness based on how well it meets carbs requirements. (upper and lower bound)
            double carbsFitness = 1 - (Math.Abs(totalCarbs - ((mealPlanRequest.maxCarbs + mealPlanRequest.minCarbs) / 2)) / (mealPlanRequest.maxCarbs - mealPlanRequest.minCarbs));


            //fitness based on how well it meets fats requirements. (upper and lower bound)
            double fatsFitness = 1 - (Math.Abs(totalFat - ((mealPlanRequest.maxFats + mealPlanRequest.minFats) / 2)) / (mealPlanRequest.maxFats - mealPlanRequest.minFats));
           
           
            //calcualte the total fitness based on all 5 parameters.
            double fitness = (MealPlanConstants.weightCalorie * calorieFitness) + (MealPlanConstants.weightProtein * proteinFitness) + (MealPlanConstants.weightCost * costFitness) + (MealPlanConstants.weightCarbs * carbsFitness) + (MealPlanConstants.weightFat * fatsFitness);
            */

            //calcualte the total fitness based on just 2 parameters.(subject to change)
            double fitness = (MealPlanConstants.weightCalorie * calorieFitness) + (MealPlanConstants.weightCost * costFitness);
            
            return fitness;

        }

        public MealPlan rouletteWheel(MealPlanPopulation population)
        {
            Random random = new Random();

            double totalFitness = population.mealPlans.Sum(m => m.fitness);
            double randomValue = random.NextDouble() * totalFitness;
            double sum = 0;

            foreach (var mealPlan in population.mealPlans)
            {
                sum += mealPlan.fitness;
                if (sum >= randomValue)
                {
                    return mealPlan;
                }
            }

            return population.mealPlans.Last();
        }


        public List<MealPlanParentPair> generateParents(MealPlanPopulation population)
        {
            //generate parent pairs. No of parent pairs will be half the size of the population. No of parents will be the size of the population. 
            //so if there are 10 individuals in a population, generate 5 pairs of parents (10 parents).

            List<MealPlanParentPair> parentpairs = new List<MealPlanParentPair>();

            for (int i = 0; i < population.populationSize / 2.0; i++)
            {
                MealPlanParentPair parents = new MealPlanParentPair
                {
                    Parent1 = rouletteWheel(population),
                    Parent2 = rouletteWheel(population),
                };

                parentpairs.Add(parents);
            }

            return parentpairs;
        }


        public MealPlanChildPair crossover(MealPlanParentPair parentPair, int chromomeLength, GenerateMealPlanRequestModel model)
        {
            Random random = new Random();
            MealPlanChildPair childPair = new MealPlanChildPair
            {
                Child1 = new MealPlan { meals = new List<Meal>() },
                Child2 = new MealPlan { meals = new List<Meal>() },
            };

            int crossoverPoint = random.Next(0, chromomeLength);
            for (int j = 0; j < chromomeLength; j++)
            {
                if (j < crossoverPoint)
                {
                    childPair.Child1.meals.Add(parentPair.Parent1.meals[j]);
                    childPair.Child2.meals.Add(parentPair.Parent2.meals[j]);

                }
                else
                {
                    childPair.Child1.meals.Add(parentPair.Parent2.meals[j]);
                    childPair.Child2.meals.Add(parentPair.Parent1.meals[j]);

                }
            }
            
            //add the additional properties to the first child and assign the fitness function.
            childPair.Child1 = calculateMealPlanProperties(childPair.Child1);
            childPair.Child1.fitness = fitnessFunction(childPair.Child1,model);


            //add the additional properties to the second child and assign the fitness function.
            childPair.Child2 = calculateMealPlanProperties(childPair.Child2);
            childPair.Child2.fitness = fitnessFunction(childPair.Child2, model);

            return childPair;

        }


        public List<MealPlanChildPair> generateOffspring(List<MealPlanParentPair> parentpairs, int numMeals, GenerateMealPlanRequestModel model)
        {
            Random random = new Random();

            List<MealPlanChildPair> offspring = new List<MealPlanChildPair>();

            foreach (var parentpair in parentpairs)
            {
                var childPair = crossover(parentpair, parentpair.Parent1.meals.Count, model);
                offspring.Add(childPair);
            }


            return offspring;

        }

        public MealPlan calculateMealPlanProperties(MealPlan mealPlan)
        {
            var totalCalories = mealPlan.meals.Sum(m => m.Calories);
            var totalCost = mealPlan.meals.Sum(m => m.Cost);
            var totalCarbs = mealPlan.meals.Sum(m => m.Carbs);
            var totalProtein = mealPlan.meals.Sum(m => m.Protein);
            var totalFat = mealPlan.meals.Sum(m => m.Fat);


            //this will be used to calculate the percentage calories from each of them for the pie chart
            var totalCaloriesFromCarbs = (totalCarbs * NutritionalConstants.carbsToCalories);
            var totalCaloriesFromProtein = (totalProtein * NutritionalConstants.proteinToCalories);
            var totalCaloriesFromFat = (totalFat * NutritionalConstants.fatToCalories);

            var totalCaloriesFromMacros = totalCaloriesFromProtein + totalCaloriesFromCarbs + totalCaloriesFromFat;


            //calculate the percentages for carbs protein and fat
            var percentCaloriesFromCarbs = (totalCaloriesFromCarbs / totalCaloriesFromMacros) * 100.0;
            var percentCaloriesFromProtein = (totalCaloriesFromProtein / totalCaloriesFromMacros) * 100.0;
            var percentCaloriesFromFat = (totalCaloriesFromFat / totalCaloriesFromMacros) * 100.0;


            //update the values of the meal plan and return the meal plan.
            mealPlan.totalProtein = totalProtein;
            mealPlan.totalFat = totalFat;
            mealPlan.totalCalories = totalCalories;
            mealPlan.totalCost = totalCost;
            mealPlan.totalCarbs = totalCarbs;
            mealPlan.percentCalorieFromFat = percentCaloriesFromFat;
            mealPlan.percentCalorieFromProtein = percentCaloriesFromProtein;
            mealPlan.percentCalorieFromCarbs = percentCaloriesFromCarbs;

            return mealPlan;

        }


        public async Task<MealPlanChildPair> mutation(MealPlanChildPair childPair, double mutationRate)
        {
            Random random = new Random();

            //mutation for the first child
            for (int j = 0; j < childPair.Child1.meals.Count; j++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    switch (j)
                    {
                        case 0:
                            await MutateBreakfast(childPair.Child1);
                            break;
                        case 1:
                            await MutateLunch(childPair.Child1);
                            break;
                        case 2:
                            await MutateDinner(childPair.Child1);
                            break;
                    }
                    break; //break out of the for loop once you have mutated a gene
                }

            }

            //mutation for the second child
            for (int j = 0; j < childPair.Child2.meals.Count; j++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    switch (j)
                    {
                        case 0:
                            await MutateBreakfast(childPair.Child2);
                            break;
                        case 1:
                            await MutateLunch(childPair.Child2);
                            break;
                        case 2:
                            await MutateDinner(childPair.Child2);
                            break;
                    }
                    break; //break out of the for loop once you have mutated a gene
                }

            }
            return childPair;

        }


        public async Task<MealPlan> MutateBreakfast(MealPlan mealPlan)
        {
            Random random = new Random();
            var breakfasts = await _mealRepository.getBreakfasts();
            Meal newBreakfast = breakfasts[random.Next(breakfasts.Count)];

            //google how to replace the food at a particular index
            mealPlan.meals.RemoveAt(0);
            mealPlan.meals.Insert(0, newBreakfast);

            return mealPlan;

        }

        public async Task<MealPlan> MutateLunch(MealPlan mealPlan)
        {
            Random random = new Random();
            var lunches = await _mealRepository.getLunches();
            Meal newLunch = lunches[random.Next(lunches.Count)];

            //google how to replace the food at a particular index
            mealPlan.meals.RemoveAt(1);
            mealPlan.meals.Insert(1, newLunch);

            return mealPlan;

        }

        public async Task<MealPlan> MutateDinner(MealPlan mealPlan)
        {
            Random random = new Random();
            var dinners = await _mealRepository.getDinners();
            Meal newDinner = dinners[random.Next(dinners.Count)];

            //google how to replace the food at a particular index
            mealPlan.meals.RemoveAt(2);
            mealPlan.meals.Insert(2, newDinner);

            return mealPlan;

        }


        //THIS METHOD IS TO REGENERATE THE MEAL PLAN FOR A DAY BASICALLY. DURATION IS ALWAYS ONE DAY
        public async Task<ApiResponse> regenerateMealPlan(GenerateMealPlanRequestModel model)
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();

            try
            {
                

                //I AM NOT YET SO SURE OF THE ABOVE TWO LINES SHA O.

                MealPlanPopulation population = new MealPlanPopulation
                {
                    mealPlans = new List<MealPlan>(),
                    populationSize = MealPlanConstants.populationSize,
                };

                FinalMealPlan finalMealPlan = new FinalMealPlan { mealPlans = new List<MealPlan>() };

              
                    //this for loop uses genetic algorithm to generate a meal plan for a single day.
                    for (int j = 0; j < MealPlanConstants.noOfGenerations; j++)
                    {

                        //STEP 1 - create initial population
                        population = await createInitialPopulation(MealPlanConstants.populationSize);


                        //STEP 2 - evaluate the fitness of each mealplan in initial population
                        foreach (var mealPlan in population.mealPlans)
                        {
                            var mealPlanFitness = fitnessFunction(mealPlan, model);
                            mealPlan.fitness = mealPlanFitness;
                        }

                        //STEP 3- SELECTION : next is to select parents which will involve roulette wheel 
                        var parentpairs = generateParents(population);


                        //STEP 4- CROSSOVER. : generate offspring by crossing over between pairs of parents
                        //use crossover to create offspring in pairs for the next gen/population.

                        var childrenpairs = generateOffspring(parentpairs, 3, model);

                        //STEP 5- MUTATION, continue from here
                        //mutate each offspring
                        foreach (var childpair in childrenpairs)
                        {
                            await mutation(childpair, MealPlanConstants.mutationRate);

                        }

                        //clear population currently and set new population to new mutated offspring
                        population.mealPlans.Clear();

                        foreach (var childpair in childrenpairs)
                        {
                            population.mealPlans.Add(childpair.Child1);
                            population.mealPlans.Add(childpair.Child2);
                        }


                    }
                    //get the meal plan in the last generation with the highest fitness- that is the final answer. that is the best meal plan FOR THE DAY. add it to final meal plan for the week or month
                    MealPlan bestMealPlan = population.mealPlans.OrderByDescending(f => f.fitness).First();
                    finalMealPlan.mealPlans.Add(bestMealPlan);


                return returnedResponse.CorrectResponse(finalMealPlan);
            }

            catch (Exception e)
            {
                return returnedResponse.ErrorResponse(e.ToString(), null);
            }

        }

        public ApiResponse getBudgetForDay()
        {
            //throw new NotImplementedException();
            ReturnedResponse returnedResponse = new ReturnedResponse();
            BudgetRange budgetRange1 = new BudgetRange
            {
                minBudget = 1000,
                maxBudget = 1500
            };

            BudgetRange budgetRange2 = new BudgetRange
            {
                minBudget = 1500,
                maxBudget = 2000
            };

            BudgetRange budgetRange3 = new BudgetRange
            {
                minBudget = 2000,
                maxBudget = 2500
            };

            BudgetRange budgetRange4 = new BudgetRange
            {
                minBudget = 2500,
                maxBudget = 3000
            };


            List<BudgetRange> budgetRanges = new List<BudgetRange>
            {
                budgetRange1, budgetRange2, budgetRange3, budgetRange4 
            };

            return returnedResponse.CorrectResponse(budgetRanges);

        }

        public ApiResponse getBudgetForWeek()
        {

            ReturnedResponse returnedResponse = new ReturnedResponse();
            BudgetRange budgetRange1 = new BudgetRange
            {
                minBudget = 7000,
                maxBudget = 9000
            };

            BudgetRange budgetRange2 = new BudgetRange
            {
                minBudget = 9000,
                maxBudget = 11000
            };

            BudgetRange budgetRange3 = new BudgetRange
            {
                minBudget = 11000,
                maxBudget = 13000
            };

            BudgetRange budgetRange4 = new BudgetRange
            {
                minBudget = 13000,
                maxBudget = 15000
            };


            List<BudgetRange> budgetRanges = new List<BudgetRange>
            {
                budgetRange1, budgetRange2, budgetRange3, budgetRange4
            };

            return returnedResponse.CorrectResponse(budgetRanges);
        }

        public ApiResponse getBudgetForMonth()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            BudgetRange budgetRange1 = new BudgetRange
            {
                minBudget = 30000,
                maxBudget = 40000
            };

            BudgetRange budgetRange2 = new BudgetRange
            {
                minBudget = 40000,
                maxBudget = 50000
            };

            BudgetRange budgetRange3 = new BudgetRange
            {
                minBudget = 50000,
                maxBudget = 60000
            };

            BudgetRange budgetRange4 = new BudgetRange
            {
                minBudget = 60000,
                maxBudget = 70000
            };


            List<BudgetRange> budgetRanges = new List<BudgetRange>
            {
                budgetRange1, budgetRange2, budgetRange3, budgetRange4
            };

            return returnedResponse.CorrectResponse(budgetRanges);
        }

        public async Task<ApiResponse> updateMealsValues()
        {
            ReturnedResponse returnedResponse = new ReturnedResponse();
            //throw new NotImplementedException();
            var meals = await _context.Meals.ToListAsync();
            foreach(var meal in meals)
            {
                meal.Protein = Math.Round(meal.Protein, 2);
                meal.Calories = Math.Round(meal.Calories, 2);
                meal.Cost = Math.Round(meal.Cost, 2);
                meal.Fat = Math.Round(meal.Fat, 2);
                meal.Carbs = Math.Round(meal.Carbs, 2);

                _context.Update(meal);
                await _context.SaveChangesAsync();
            }

            return returnedResponse.CorrectResponse("successful");
        }
    }
}
