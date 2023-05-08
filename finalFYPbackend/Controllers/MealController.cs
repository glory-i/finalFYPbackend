using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using finalFYPbackend.Model;
using Microsoft.AspNetCore.Authorization;

namespace finalFYPbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealServices _mealServices;
        public MealController(IMealServices mealServices)
        {
            _mealServices = mealServices;
        }

        [HttpPost("CreateMeal")]
        public async Task<ActionResult<ApiResponse>> CreateMeal(createMealDTO model)
        {

            var response = await _mealServices.createMeal(model);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        //I need to randomize the "popular breakfasts"
        [HttpGet("PopularBreakfast")]
        public async Task<ActionResult<ApiResponse>> PopularBreakfast()
        {

            var response = await _mealServices.popularBreakfasts();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        //I need to randomize the "popular lunches"
        [HttpGet("PopularLunch")]
        public async Task<ActionResult<ApiResponse>> PopularLunch()
        {

            var response = await _mealServices.popularLunches();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        //I need to randomize the "popular dinners"
        [HttpGet("PopularDinner")]
        public async Task<ActionResult<ApiResponse>> PopularDinner()
        {

            var response = await _mealServices.popularDinners();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpGet("SearchMeals")]
        public async Task<ActionResult<ApiResponse>> SearchMeals(string search)
        {

            var response = await _mealServices.searchMeals(search);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpGet("GetMeals")]
        public async Task<ActionResult<ApiResponse>> GetMeals(string mealType, string search)
        {

            var response = await _mealServices.getMeals(mealType, search);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        ///[Authorize]
        [HttpPost("GenerateMealPlan")]
        public async Task<ActionResult<ApiResponse>> GenerateMealPlan(string duration,[FromBody] GenerateMealPlanRequestModel model)
        {

            var response = await _mealServices.generateMealPlan(duration, model);
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
