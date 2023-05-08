using finalFYPbackend.Model.NutritionModels;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace finalFYPbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionServices _nutritionService;
        public NutritionController(INutritionServices nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpPost("CalculateNutritionRequirements")]
        public async Task<ActionResult<ApiResponse>> CalculateNutritionRequirements(NutritionCalculatorRequestModel model)
        {

            var response = await _nutritionService.NutritionCalculator(model);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}
