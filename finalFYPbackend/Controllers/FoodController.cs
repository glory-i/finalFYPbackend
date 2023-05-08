using finalFYPbackend.DTOs.MealDTOs;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Implementation;
using finalFYPbackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using finalFYPbackend.Authentication;
using finalFYPbackend.Model;

namespace finalFYPbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodServices _foodServices;
        public FoodController(IFoodServices foodServices)
        {
            _foodServices = foodServices;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateFood")]
        public async Task<ActionResult<ApiResponse>> CreateMeal(createFoodDTO model)
        {
            var response = await _foodServices.createFood(model);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("UpdateFood")]
        public async Task<ActionResult<ApiResponse>> UpdateFood(int foodId, string imageUrl)
        {
            var response = await _foodServices.updateFood(foodId, imageUrl);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

       /* [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateBreadAndEgg")]
        public async Task<ActionResult<ApiResponse>> CreateEwagMeal()
        {
            var response = await _foodServices.createBreadandEggMeals();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        } */

        //for now i am done with this endpoint
        /*
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateHephzibahMeals")]
        public async Task<ActionResult<ApiResponse>> CreateHephzibahMeals()
        {
            var response = await _foodServices.createHephzibahMeals();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        */

        ///for now i am done with this endpoint
        /* [Authorize(Roles = UserRoles.Admin)]
         [HttpPost("CreateSharonMeals")]
         public async Task<ActionResult<ApiResponse>> CreateSharonMeals()
         {
             var response = await _foodServices.createSharonMeals();
             if (response.Message == ApiResponseEnum.success.ToString())
             {
                 return Ok(response);
             }
             else
             {
                 return BadRequest(response);
             }

         } */


        //for now i am done with this endpoint
        /* [Authorize(Roles = UserRoles.Admin)]
         [HttpPost("CreateOliveYardMeals")]
         public async Task<ActionResult<ApiResponse>> CreateOliveYardMeals()
         {
             var response = await _foodServices.createOliveYardMeals();
             if (response.Message == ApiResponseEnum.success.ToString())
             {
                 return Ok(response);
             }
             else
             {
                 return BadRequest(response);
             }

         }*/

        // I am done with this endpoint for now
        /*[Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateBashanMeals")]
        public async Task<ActionResult<ApiResponse>> CreateBashanMeals()
        {
            var response = await _foodServices.createBashanMeals();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        } */

        //I am done with this endpoint for now
        /*
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateWaakyeandSpagMeals")]
        public async Task<ActionResult<ApiResponse>> CreateWaakyeandSpagMeals()
        {
            var response = await _foodServices.createWaakyeAndSpagMeals();
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        */

    }
}
