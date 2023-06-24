using System.Collections.Generic;

namespace finalFYPbackend.Model
{
    //in context of genetic algorithm, this is a chromosome. check figma for properties.
    public class MealPlan
    {
        public List<Meal> meals { get; set; }
        public double fitness { get; set; }

        public double totalCalories { get; set; }
        public double totalCarbs { get; set; }
        public double totalProtein { get; set; }
        public double totalFat { get; set; }
        public double totalCost { get; set; }
        public double percentCalorieFromFat { get; set; }
        public double percentCalorieFromCarbs { get; set; }
        public double percentCalorieFromProtein { get; set; }
        
        
        
        
        public double targetMinCost { get; set; }
        public double targetMaxCost { get; set; }
        public double targetCalories { get; set; }
        public double targetMinCarbs { get; set; }
        public double targetMaxCarbs { get; set; }

        public double targetMinProtein { get; set; }
        public double targetMaxProtein { get; set; }

        public double targetMinFat { get; set; }
        public double targetMaxFat { get; set; }



    }


    public class MealPlanPopulation
    {
        public List<MealPlan> mealPlans { get; set; }
        public int populationSize { get; set; }

    }

    //the mealplan request model - gotten directly from the flutter
    public class GenerateMealPlanRequestModel
    {
        public double minBudget { get; set; }
        public double maxBudget { get; set; }
        public double calorieRequirements { get; set; }

    }


    //I will calculate myself. 
    public class GenerateMealPlanRequest : GenerateMealPlanRequestModel
    {
        public double minProtein { get; set; }
        public double maxProtein { get; set; }
        public double minCarbs { get; set; }
        public double maxCarbs { get; set; }
        public double minFats { get; set; }
        public double maxFats { get; set; }

    }


    //a pair of parents in the genetic algorithm
    public class MealPlanParentPair
    {
        public MealPlan Parent1 { get; set; }
        public MealPlan Parent2 { get; set; }
    }


    //a pair of children in the genetic algorithm
    public class MealPlanChildPair
    {
        public MealPlan Child1 { get; set; }
        public MealPlan Child2 { get; set; }
    }


    public class FinalMealPlan
    {
        public List <MealPlan> mealPlans { get; set; }
    }
}
