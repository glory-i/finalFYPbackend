using System.ComponentModel;

namespace finalFYPbackend.Responses.Enums
{
    public enum HeightUnitEnum
    {
        [Description("feet")] feet = 1,
        [Description("meters")] meters,
    }

    public enum GenderEnum
    {
        [Description("Male")] Male = 1,
        [Description("Female")] Female,
    }


    public enum GoalEnum
    {
        [Description("Lose Weight")] Lose = 1,
        [Description("Gain Weight")] Gain = 2,
        [Description("Maintain Weight")] Maintain = 3,
    }

    public enum ActivityLevelEnum
    {
        [Description("Sedentary")] Sedentary = 1,
        [Description("Lightly Active")] LightlyActive = 2,
        [Description("Moderately Active")] ModeratelyActive = 3,
        [Description("Active")] Active = 4,
        [Description("Very Active")] VeryActive = 5,
    }




    public enum FoodTypeEnum
    {
        [Description("Breakfast")] Breakfast = 1,
        [Description("Lunch")] Lunch = 2,
        [Description("Dinner")] Dinner = 3,
        [Description("Breakfast Lunch")] BreakfastLunch = 4,
        [Description("Breakfast Dinner")] BreakfastDinner = 5,
        [Description("Lunch Dinner")] LunchDinner = 6,
        [Description("Breakfast Lunch Dinner")] BreakfastLunchDinner = 7,

    }

    public enum DurationEnum
    {
        [Description("Day")] Day = 1,
        [Description("Week")] Week = 2,
        [Description("Month")] Month = 3,
    }



    public enum FoodCategoryEnum
    {
        [Description("Primary")] Primary = 1,
        [Description("Secondary")] Secondary = 2,
        [Description("Protein")] Protein = 3,
        [Description("Primary Secondary")] PrimarySecondary = 4,
        [Description("Swallow")] Swallow = 5,
        [Description("Soup")] Soup = 6,
        [Description("Stew")] Stew = 7,
        [Description("Miscellaneous")] Miscellaneous = 8,
        [Description("Tertiary")] Tertiary = 9,

    }

    public enum RestaurantEnum
    {
        [Description("Hephzibah Restaurant")] HephzibahRestaurant = 1,
        [Description("OliveYard Restaurant")] OliveYardRestaurant = 2,
        [Description("Sharon Restaurant")] SharonRestaurant = 3,
        [Description("Bashan Restaurant")] BashanRestaurant = 4,
        [Description("Waakye and Spag Restaurant")] WaakyeandSpagRestaurant = 5,
        [Description("Ewa G Stand")] EwaGStand = 6,
        [Description("Bread and Egg Stand")] BreadandEggStand = 7,
    }

}
