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

namespace ProductMicroServices.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRL _productRL;
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration Configuration;
        public ProductController(IProductRL productRL, ILogger<ProductController> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _productRL = productRL;
            _logger = logger;
            
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] AddProductRequest request)
        {
            AddProductResponse response = new AddProductResponse();
            try
            {
                _logger.LogInformation($"AddProduct Calling In ProductController");
                response = await _productRL.AddProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllProduct(GetAllProductRequest request)
        {
            GetAllProductResponse response = new GetAllProductResponse();
            try
            {
                _logger.LogInformation($"GetAllProduct Calling In ProductController");
                response = await _productRL.GetAllProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByID([FromQuery] GetProductByIDRequest request)
        {
            GetProductByIDResponse response = new GetProductByIDResponse();
            try
            {
                _logger.LogInformation($"GetProductByID Calling In ProductController");
                response = await _productRL.GetProductByID(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByName([FromQuery] GetProductByNameRequest request)
        {
            GetProductByNameResponse response = new GetProductByNameResponse();
            try
            {
                _logger.LogInformation($"GetProductByName Calling In ProductController");
                response = await _productRL.GetProductByName(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            UpdateProductResponse response = new UpdateProductResponse();
            try
            {
                _logger.LogInformation($"UpdateProduct Calling In ProductController");
                response = await _productRL.UpdateProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> ProductMoveToArchive(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            try
            {
                _logger.LogInformation($"ProductMoveToArchive Calling In ProductController");
                response = await _productRL.ProductMoveToArchive(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetArchiveProduct(GetAllProductRequest request)
        {
            GetArchiveProductResponse response = new GetArchiveProductResponse();
            try
            {
                _logger.LogInformation($"GetArchiveProduct Calling In ProductController");
                response = await _productRL.GetArchiveProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> ProductMoveToTrash(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            try
            {
                _logger.LogInformation($"ProductMoveToTrash Calling In ProductController");
                response = await _productRL.ProductMoveToTrash(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetTrashProduct(GetAllProductRequest request)
        {
            GetArchiveProductResponse response = new GetArchiveProductResponse();
            try
            {
                _logger.LogInformation($"GetTrashProduct Calling In ProductController");
                response = await _productRL.GetTrashProduct(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> ProductDeletePermenently(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            try
            {
                _logger.LogInformation($"ProductDeletePermenently Calling In ProductController");
                response = await _productRL.ProductDeletePermenently(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> ProductRestore(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            try
            {
                _logger.LogInformation($"ProductRestore Calling In ProductController");
                response = await _productRL.ProductRestore(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs In ProductController. Message : {ex.Message}");
            }

            return Ok(response);
        }


    }

}
