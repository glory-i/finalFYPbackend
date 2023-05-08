namespace finalFYPbackend.Model.NutritionModels
{
    public class NutritionCalculatorRequestModel
    {
        public int Age { get; set; }
        public string Gender { get; set; }

        public double Weight { get; set; }
        public double HeightInFeet { get; set; }
        public double HeightInInches { get; set; }
        public string ActivityLevel { get; set; }
        public string Goal { get; set; }
    }



    public class NutritionCalculatorResponseModel
    {
        public double totalCaloriesRequired { get; set; }

        //min g of protein needed. recall that 1g of proteitn = 4 calories
        public double minProteinRequired { get; set; }

        //max g of protein needed. recall that 1g of proteitn = 4 calories
        public double maxProteinRequired { get; set; }

        //min g of carbs needed. recall that 1g of carbs = 4 calories
        public double minCarbsRequired { get; set; }

        //max g of carbs needed. recall that 1g of carbs = 4 calories
        public double maxCarbsRequired { get; set; }

        //min g of fats needed. recall that 1g of fat = 9 calories
        public double minFatRequired { get; set; }

        //max g of fats needed. recall that 1g of fat = 9 calories
        public double maxFatRequired { get; set; }
    }

}
