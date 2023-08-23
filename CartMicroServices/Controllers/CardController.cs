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

namespace CartMicroServices.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardRL _cardRL;
        private readonly ILogger<CardController> _logger;
        private readonly IConfiguration Configuration;
        public CardController(ICardRL cardRL, ILogger<CardController> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _cardRL = cardRL;
            _logger = logger;
            
        }

        [HttpPost]
        public async Task<IActionResult> AddToCard(AddToCardRequest request)
        {
            AddToCardResponse response = new AddToCardResponse();
            try
            {
                _logger.LogInformation($"AddToCard Calling In CardController ");
                response =  await _cardRL.AddToCard(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCardDetails(GetAllCardDetailsRequest request)
        {
            GetAllCardDetailsResponse response = new GetAllCardDetailsResponse();
            try
            {
                _logger.LogInformation($"GetAllCardDetails Calling In CardController ");
                response =  await _cardRL.GetAllCardDetails(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCartProduct(RemoveCardRequest request)
        {
            RemoveCardResponse response = new RemoveCardResponse();
            try
            {
                _logger.LogInformation($"RemoveCartProduct Calling In CardController ");
                response = await _cardRL.RemoveCartProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> OrderProduct(OrderProductRequest request)
        {
            OrderProductResponse response = new OrderProductResponse();
            try
            {
                _logger.LogInformation($"OrderProduct Calling In CardController ");
                response = await _cardRL.OrderProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderProduct(GetOrderProductRequest request)
        {
            GetOrderProductResponse response = new GetOrderProductResponse();
            try
            {
                _logger.LogInformation($"GetOrderProduct Calling In CardController ");
                response = await _cardRL.GetOrderProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> CancleOrder(OrderProductRequest request)
        {
            OrderProductResponse response = new OrderProductResponse();
            try
            {
                _logger.LogInformation($"CancleOrder Calling In CardController ");
                response = await _cardRL.CancleOrder(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> Rating(AddRatingRequest request)
        {
            AddRatingResponse response = new AddRatingResponse();
            try
            {
                _logger.LogInformation($"AddRating Calling In CardController ");
                response = await _cardRL.AddRating(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In CardController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> PaymentGetway(PaymentGetwayRequest request)
        {
            PaymentGetwayResponse response= new PaymentGetwayResponse();

            try
            {
                response = await _cardRL.PaymentGetway(request);

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : "+ ex.Message;
            }

            return Ok(response);
        }

    }
}
