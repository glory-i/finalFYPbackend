using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace finalFYPbackend.DTOs.MealDTOs
{
    public class createMealDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeOfMeal { get; set; }
        public string Producer { get; set; }
        public double Cost { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }

        public string imageUrl { get; set; }

        public string FoodImageData { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }

    public class createFoodDTO
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeOfMeal { get; set; } //breakfast lunch dinner
        public string CategoryOfFood { get; set; } //swallow soup primaryfood, secondary food, stew etc

        public string Producer { get; set; }
        public double Cost { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string imageUrl { get; set; }
        public string flutterImageUrl { get; set; } //image url that can be recognized by flutter

        public string FoodImageData { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }

}
