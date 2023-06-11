using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace finalFYPbackend.Model
{
    public class Meal
    {
        public int id { get; set; }
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
        public string flutterImageUrl { get; set; } //image url that can be recognized by flutter

       // public string FoodImageData { get; set; }

       // [NotMapped]
       // [DisplayName("Upload File")]
        //public IFormFile ImageFile { get; set; }
    }

}
