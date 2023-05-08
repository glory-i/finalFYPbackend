namespace finalFYPbackend.Utilities
{
    public static class NutritionalConstants
    {
        public static double carbsToCalories = 4.0;
        public static double proteinToCalories = 4.0;
        public static double fatToCalories = 9.0;



        /// <summary>
        /// ALL VALUES HERE ARE SUBJECT TO CHANGE BASED ON RESEARCH
        /// </summary>
        public static double minPercentCaloriesFromCarbs = 10.0;
        public static double maxPercentCaloriesFromCarbs = 35.0;

        public static double minPercentCaloriesFromProteins = 45.0;
        public static double maxPercentCaloriesFromProteins = 65.0;

        public static double minPercentCaloriesFromFats = 30.0;
        public static double maxPercentCaloriesFromFats = 40.0;


    }

    public static class ImageConstants
    {
        public static string jpgImageData = "data:image/jpg;base64,";
    }

    public static class MealPlanConstants
    {
        public static int populationSize = 10;

        //weights that will be used to calclate the fitness function. subject to  change 
        public static double weightCalorie = 0.5;
        public static double weightCost = 0.5;
        public static double weightCarbs = 0.1;
        public static double weightProtein = 0.1;
        public static double weightFat = 0.1;
        
        
        public static double mutationRate = 0.5;
        public static double noOfGenerations = 10;

    }


    public static class AdminConstants
    {
        public static string username = "FoodAdmin";
        public static string password = "Admin@1234";
    }

    
}
