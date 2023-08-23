using CommonLayer.Models;
using Inventory_Management_System_BE.Common_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryLayers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishListMicroServices.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListRL _wishListRL;
        private readonly ILogger<WishListController> _logger;
        private readonly IConfiguration Configuration;
        public WishListController(IWishListRL wishListRL, ILogger<WishListController> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _wishListRL = wishListRL;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> AddToWishList(AddToWishListRequest request)
        {
            AddToWishListResponse response = new AddToWishListResponse();
            try
            {
                _logger.LogInformation($"AddToWishList Calling In WishListController");
                response = await _wishListRL.AddToWishList(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In WishListController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllWishListDetails(GetAllWishListDetailsRequest request)
        {
            GetAllWishListDetailsResponse response = new GetAllWishListDetailsResponse();
            try
            {
                _logger.LogInformation($"GetAllWishListDetails Calling In WishListController");
                response = await _wishListRL.GetAllWishListDetails(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In WishListController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveWishListProduct(RemoveWishListProductRequest request)
        {
            RemoveWishListProductResponse response = new RemoveWishListProductResponse();
            try
            {
                _logger.LogInformation($"RemoveWishListProduct Calling In WishListController");
                response = await _wishListRL.RemoveWishListProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In WishListController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> MoveToCard(MoveToCardRequest request)
        {
            MoveToCardResponse response = new MoveToCardResponse();
            try
            {
                _logger.LogInformation($"MoveToCard Calling In WishListController");
                response = await _wishListRL.MoveToCard(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In WishListController. Message : {ex.Message}");
            }

            return Ok(response);
        }
    }

}
