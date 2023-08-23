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

namespace FeedbackMicroServices.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRL _feedbackRL;
        private readonly ILogger<FeedbackController> _logger;
        private readonly IConfiguration Configuration;
        public FeedbackController(ILogger<FeedbackController> logger, IFeedbackRL feedbackRL, IConfiguration configuration)
        {
            Configuration = configuration;
            _feedbackRL = feedbackRL;
            _logger = logger;
            
        }

        [HttpPost]
        public async Task<ActionResult> GetFeedbacks(GetFeedbacksRequest request)
        {
            GetFeedbacksResponse response = new GetFeedbacksResponse();
            try
            {
                _logger.LogInformation($"GetFeedbacks Calling In FeedbackController.... Time : {DateTime.Now}");
                response = await _feedbackRL.GetFeedbacks(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In FeedbackController : Message : ", ex.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddFeedback(AddFeedbackRequest request)
        {
            AddFeedbackResponse response = new AddFeedbackResponse();
            try
            {
                _logger.LogInformation($"AddFeedback Calling In FeedbackController.... Time : {DateTime.Now}");
                response = await _feedbackRL.AddFeedback(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In FeedbackController : Message : ", ex.Message);
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFeedback([FromQuery] int ID)
        {
            DeleteFeedbackResponse response = new DeleteFeedbackResponse();
            try
            {
                _logger.LogInformation($"DeleteFeedback Calling In FeedbackController.... Time : {DateTime.Now}");
                response = await _feedbackRL.DeleteFeedback(ID);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In FeedbackController : Message : ", ex.Message);
            }

            return Ok(response);
        }
    }

}
