using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayers.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Services
{
    public class ProductRL : IProductRL
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _SqlConnection;
        private readonly ILogger<ProductRL> _logger;
        private readonly ApplicationDbContext _dbContext;
        public ProductRL(IConfiguration configuration, ApplicationDbContext dbContext, ILogger<ProductRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _SqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
        }

        public async Task<AddProductResponse> AddProduct(AddProductRequest request)
        {
            AddProductResponse response = new AddProductResponse();
            response.Message = "Add Product Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"AddProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                Account account = new Account(
                                _configuration["CloudinarySettings:CloudName"],
                                _configuration["CloudinarySettings:ApiKey"],
                                _configuration["CloudinarySettings:ApiSecret"]);

                var path = request.File.OpenReadStream();

                Cloudinary cloudinary = new Cloudinary(account);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(request.File.FileName, path),
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                string SqlQuery = @"
                                    INSERT INTO ProductDetails
                                    (ProductName,ProductType,ProductPrice,ProductDetails,ProductCompany,Quantity,ProductImageUrl,PublicId) VALUES
                                    (@ProductName,@ProductType,@ProductPrice,@ProductDetails,@ProductCompany,@Quantity,@ProductImageUrl,@PublicId);
                                    ";

                RatingCalculation ratingCalculation = new RatingCalculation();
                ratingCalculation.data = new List<UserCount>();

                ProductDetails productDetails = new ProductDetails()
                {
                    ProductName = request.ProductName,
                    ProductType = request.ProductType,
                    ProductPrice = request.ProductPrice,
                    ProductDetail = request.ProductDetails,
                    ProductCompany = request.ProductCompany,
                    Quantity = request.Quantity,
                    ProductImageUrl = uploadResult.Url.ToString(),
                    PublicId = uploadResult.PublicId.ToString(),
                    Rating = JsonConvert.SerializeObject(ratingCalculation)
                };

                await _dbContext.AddAsync(productDetails);
                var Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.Message = "Something Went Wrong";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message 2 : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetAllProductResponse> GetAllProduct(GetAllProductRequest request)
        {
            GetAllProductResponse response = new GetAllProductResponse();
            response.Message = "Fetch Product Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetAllProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                /*if (_SqlConnection != null && _SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _SqlConnection.OpenAsync();
                }*/

                string SqlQuery = @"
                                    SELECT * ,
                                    (select COUNT(*) from ProductDetails Where IsActive=1 And IsArchive=0) As TotalRecord
                                    FROM ProductDetails
                                    Where IsActive=1 And IsArchive=0
                                    ORDER BY ProductID DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY";


                var ProductDetails = _dbContext.ProductDetails
                                                    .Where(X => X.IsActive == true && X.IsArchive == false)
                                                    .OrderByDescending(X => X.ProductID)
                                                    .Skip((request.PageNumber - 1) * request.NumberOfRecordPerPage)
                                                    .Take(request.NumberOfRecordPerPage)
                                                    .ToList();

                if(ProductDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Product Not Found";
                    return response;
                }

                response.data = new List<GetAllProduct>();
                foreach (var Product in ProductDetails)
                {
                    GetAllProduct data = new GetAllProduct();
                    data.InsertionDate = Convert.ToDateTime(Product.InsertionDate).ToString("dddd, dd-MMM-yyyy, HH:mm tt");
                    data.IsActive = Product.IsActive;
                    data.IsArchive = Product.IsArchive;
                    data.ProductCompany = Product.ProductCompany;
                    data.ProductDetails = Product.ProductDetail;
                    data.ProductID = Product.ProductID;
                    data.ProductImageUrl = Product.ProductImageUrl;
                    data.ProductName = Product.ProductName;
                    data.ProductPrice = Product.ProductPrice;
                    data.ProductType = Product.ProductType;
                    data.PublicID = Product.PublicId;
                    data.Quantity = Product.Quantity;
                    data.Rating = JsonConvert.DeserializeObject<RatingCalculation>(Product.Rating).TotalRating;
                    response.data.Add(data);
                }

                response.TotalRecords = _dbContext.ProductDetails.Where(X => X.IsActive == true && X.IsArchive == false).Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message 3 : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetProductByIDResponse> GetProductByID(GetProductByIDRequest request)
        {
            GetProductByIDResponse response = new GetProductByIDResponse();
            response.Message = "Product Found";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetProductByID In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    SELECT * 
                                    FROM ProductDetails
                                    WHERE ProductID=@ProductID
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X=>X.ProductID==request.ProductID)
                                                    .FirstOrDefault();

                if(Product == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No Product Found";
                    return response;
                }

                response.data = new GetAllProduct();

                response.data.InsertionDate = Convert.ToDateTime(Product.InsertionDate).ToString("dddd, dd-MMM-yyyy, HH:mm tt");
                response.data.IsActive = Product.IsActive;
                response.data.IsArchive = Product.IsArchive;
                response.data.ProductCompany = Product.ProductCompany;
                response.data.ProductDetails = Product.ProductDetail;
                response.data.ProductID = Product.ProductID;
                response.data.ProductImageUrl = Product.ProductImageUrl;
                response.data.ProductName = Product.ProductName;
                response.data.ProductPrice = Product.ProductPrice;
                response.data.ProductType = Product.ProductType;
                response.data.PublicID = Product.PublicId;
                response.data.Quantity = Product.Quantity;

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetProductByNameResponse> GetProductByName(GetProductByNameRequest request)
        {
            GetProductByNameResponse response = new GetProductByNameResponse();
            response.Message = "Product Found";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetProductByName In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    SELECT * 
                                    FROM ProductDetails
                                    WHERE ProductName=@ProductName
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductName == request.ProductName)
                                                    .FirstOrDefault();

                if (Product == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No Product Found";
                    return response;
                }

                response.data = new GetAllProduct();

                response.data.InsertionDate = Convert.ToDateTime(Product.InsertionDate).ToString("dddd, dd-MMM-yyyy, HH:mm tt");
                response.data.IsActive = Product.IsActive;
                response.data.IsArchive = Product.IsArchive;
                response.data.ProductCompany = Product.ProductCompany;
                response.data.ProductDetails = Product.ProductDetail;
                response.data.ProductID = Product.ProductID;
                response.data.ProductImageUrl = Product.ProductImageUrl;
                response.data.ProductName = Product.ProductName;
                response.data.ProductPrice = Product.ProductPrice;
                response.data.ProductType = Product.ProductType;
                response.data.PublicID = Product.PublicId;
                response.data.Quantity = Product.Quantity;

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<ProductMoveToArchiveResponse> ProductDeletePermenently(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            response.Message = "Product Deleted";
            response.IsSuccess = true;
            string PublicID = string.Empty;
            try
            {
                _logger.LogInformation($"ProductDeletePermenently In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                Account account = new Account(
                                _configuration["CloudinarySettings:CloudName"],
                                _configuration["CloudinarySettings:ApiKey"],
                                _configuration["CloudinarySettings:ApiSecret"]);


                Cloudinary cloudinary = new Cloudinary(account);

                string SqlQuery = @"
                                    SELECT PublicId
                                    FROM ProductDetails
                                    WHERE ProductID=@ProductID
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductID == request.ProductID)
                                                    .FirstOrDefault();

                if(Product == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product Not Found";
                    return response;
                }

                PublicID = Product.PublicId;

                var deletionParams = new DeletionParams(PublicID)
                {
                    ResourceType = ResourceType.Image
                };

                var deletionResult = cloudinary.Destroy(deletionParams);
                string Result = deletionResult.Result.ToString();
                if (Result.ToLower() != "ok")
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went To Wrong In Cloudinary Destroy Method";
                    return response;
                }

                SqlQuery = @"DELETE FROM ProductDetails WHERE ProductID=@ProductID";

                 _dbContext.ProductDetails.Remove(Product);
                int DeleteResult = await _dbContext.SaveChangesAsync();
                if (DeleteResult <= 0)
                {
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<ProductMoveToArchiveResponse> ProductMoveToArchive(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            response.Message = "Move To Archive Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"ProductMoveToArchive In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    UPDATE ProductDetails
                                    SET IsArchive=1 , IsActive=1
                                    WHERE ProductID=@ProductID
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductID == request.ProductID)
                                                    .FirstOrDefault();
                if(Product == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product Not Found";
                    return response;
                }

                Product.IsArchive = true;
                Product.IsActive = true;

                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetArchiveProductResponse> GetArchiveProduct(GetAllProductRequest request)
        {
            GetArchiveProductResponse response = new GetArchiveProductResponse();
            response.Message = "Fetch Archive Product Successfully.";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetArchiveProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    SELECT * ,
                                    (select COUNT(*) from ProductDetails WHERE IsArchive=1 AND IsActive=1) As TotalRecord
                                    FROM ProductDetails
                                    WHERE IsArchive=1 AND IsActive=1
                                    ORDER BY ProductID DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY";

                var ProductDetails = _dbContext.ProductDetails
                                                     .Where(X => X.IsActive == true && X.IsArchive == true)
                                                     .OrderByDescending(X => X.ProductID)
                                                     .Skip((request.PageNumber - 1) * request.NumberOfRecordPerPage)
                                                     .Take(request.NumberOfRecordPerPage)
                                                     .ToList();

                if (ProductDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Archive Is Empty";
                    return response;
                }

                response.data = new List<GetAllProduct>();
                foreach (var Product in ProductDetails)
                {
                    GetAllProduct data = new GetAllProduct();
                    data.InsertionDate = Convert.ToDateTime(Product.InsertionDate).ToString("dddd, dd-MMM-yyyy, HH:mm tt");
                    data.IsActive = Product.IsActive;
                    data.IsArchive = Product.IsArchive;
                    data.ProductCompany = Product.ProductCompany;
                    data.ProductDetails = Product.ProductDetail;
                    data.ProductID = Product.ProductID;
                    data.ProductImageUrl = Product.ProductImageUrl;
                    data.ProductName = Product.ProductName;
                    data.ProductPrice = Product.ProductPrice;
                    data.ProductType = Product.ProductType;
                    data.PublicID = Product.PublicId;
                    data.Quantity = Product.Quantity;
                    response.data.Add(data);
                }

                response.TotalRecords = _dbContext.ProductDetails.Where(X => X.IsActive == true && X.IsArchive == true).Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message 3 : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<ProductMoveToArchiveResponse> ProductMoveToTrash(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            response.Message = "Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"ProductMoveToTrash In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    UPDATE ProductDetails
                                    SET IsArchive=0 ,IsActive=0
                                    WHERE ProductID=@ProductID
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductID == request.ProductID)
                                                    .FirstOrDefault();
                if (Product == null)
                {
                    response.Message = "Product Not Found";
                    return response;
                }

                Product.IsArchive = false;
                Product.IsActive = false;

                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetArchiveProductResponse> GetTrashProduct(GetAllProductRequest request)
        {
            GetArchiveProductResponse response = new GetArchiveProductResponse();
            response.Message = "Fetch Trash Product Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetTrashProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    SELECT * ,
                                    (select COUNT(*) from ProductDetails WHERE IsArchive=0 AND IsActive=0) As TotalRecord
                                    FROM ProductDetails
                                    WHERE IsArchive=0 AND IsActive=0
                                    ORDER BY ProductID DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY";

                var ProductDetails = _dbContext.ProductDetails
                                                    .Where(X => X.IsActive == false && X.IsArchive == false)
                                                    .OrderByDescending(X => X.ProductID)
                                                    .Skip((request.PageNumber - 1) * request.NumberOfRecordPerPage)
                                                    .Take(request.NumberOfRecordPerPage)
                                                    .ToList();

                if (ProductDetails.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Trash Is Empty";
                    return response;
                }

                response.data = new List<GetAllProduct>();
                foreach (var Product in ProductDetails)
                {
                    GetAllProduct data = new GetAllProduct();
                    data.InsertionDate = Convert.ToDateTime(Product.InsertionDate).ToString("dddd, dd-MMM-yyyy, HH:mm tt");
                    data.IsActive = Product.IsActive;
                    data.IsArchive = Product.IsArchive;
                    data.ProductCompany = Product.ProductCompany;
                    data.ProductDetails = Product.ProductDetail;
                    data.ProductID = Product.ProductID;
                    data.ProductImageUrl = Product.ProductImageUrl;
                    data.ProductName = Product.ProductName;
                    data.ProductPrice = Product.ProductPrice;
                    data.ProductType = Product.ProductType;
                    data.PublicID = Product.PublicId;
                    data.Quantity = Product.Quantity;
                    response.data.Add(data);
                }

                response.TotalRecords = _dbContext.ProductDetails.Where(X => X.IsActive == false && X.IsArchive == false).Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message 3 : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<ProductMoveToArchiveResponse> ProductRestore(ProductMoveToArchiveRequest request)
        {
            ProductMoveToArchiveResponse response = new ProductMoveToArchiveResponse();
            response.Message = "Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"ProductRestore In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                /*if (_SqlConnection != null && _SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _SqlConnection.OpenAsync();
                }*/
                string SqlQuery = @"
                                    UPDATE ProductDetails
                                    SET IsArchive=0 , IsActive=1
                                    WHERE ProductID=@ProductID
                                    ";

                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductID == request.ProductID)
                                                    .FirstOrDefault();
                if (Product == null)
                {
                    response.Message = "Product Not Found";
                    return response;
                }

                
                Product.IsArchive = false;
                Product.IsActive = true;

                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request)
        {
            UpdateProductResponse response = new UpdateProductResponse();
            response.Message = "Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"UpdateProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                if (_SqlConnection != null && _SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _SqlConnection.OpenAsync();
                }
                string SqlQuery = @"
                                    UPDATE ProductDetails
                                    SET ProductName=@ProductName,
                                        UpdateDate=@UpdateDate,
	                                    ProductType=@ProductType,
	                                    ProductPrice=@ProductPrice,
	                                    ProductDetails=@ProductDetails,
	                                    ProductCompany=@ProductCompany,
	                                    Quantity=@Quantity
                                    WHERE ProductID=@ProductID
                                    ";
                var Product = _dbContext.ProductDetails
                                                    .Where(X => X.ProductID == request.ProductID)
                                                    .FirstOrDefault();
                if (Product == null)
                {
                    response.Message = "Product Not Found";
                    return response;
                }

                Product.ProductName = request.ProductName;
                Product.UpdateDate = DateTime.Now;
                Product.ProductType = request.ProductType;
                Product.ProductPrice = request.ProductPrice;
                Product.ProductDetail = request.ProductDetails;
                Product.ProductCompany = request.ProductCompany;
                Product.Quantity = request.Quantity;

                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                response.IsSuccess = false;
                _logger.LogError($"Exception Occurs : Message : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

    }
}
