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
    public class WishListRL : IWishListRL
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection _SqlConnection;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WishListRL> _logger;
        public WishListRL(IConfiguration configuration, ApplicationDbContext dbContext, ILogger<WishListRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _SqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
        }

        public async Task<AddToWishListResponse> AddToWishList(AddToWishListRequest request)
        {
            AddToWishListResponse response = new AddToWishListResponse();
            response.Message = "Add To WishList Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"AddToWishList In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = "INSERT INTO WishListDetails (UserId, ProductID) VALUES (@UserId, @ProductID);";

                WishListDetails wishListDetails = new WishListDetails()
                {
                    UserId = request.UserID,
                    ProductID = request.ProductID
                };

                await _dbContext.AddAsync(wishListDetails);
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

        // Inner Join
        public async Task<GetAllWishListDetailsResponse> GetAllWishListDetails(GetAllWishListDetailsRequest request)
        {
            GetAllWishListDetailsResponse response = new GetAllWishListDetailsResponse();
            response.Message = "Fetch All Wishlist Successfully";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetAllWishListDetails In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"
                                    SELECT Distinct C.WishListID, P.ProductID, 
                                                    P.InsertionDate, 
                                                    P.ProductName, 
                                                    P.ProductType , 
                                                    P.ProductPrice, 
                                                    P.ProductDetails, 
                                                    P.ProductCompany, 
                                                    P.Quantity, 
                                                    P.ProductImageUrl,
                                                    P.PublicID,
                                                    P.IsArchive, 
                                                    P.IsActive,
                                                    (SELECT COUNT(*) FROM WishListDetails WHERE UserID=@UserID) AS TotalRecord
                                    FROM WishListDetails C 
                                    INNER JOIN ProductDetails P
                                    On C.ProductID = P.ProductID
                                    WHERE  UserID=@UserID
                                    ORDER BY P.InsertionDate DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;
                                    ";

                var Result = (from c in _dbContext.WishListDetails
                              join P in _dbContext.ProductDetails
                              on c.ProductID equals P.ProductID
                              where c.UserId == request.UserID
                              select new
                              {
                                  WishListID = c.WishListID,
                                  UserID = c.UserId,
                                  ProductID = P.ProductID,
                                  InsertionDate = P.InsertionDate,
                                  ProductName = P.ProductName,
                                  ProductType = P.ProductType,
                                  ProductPrice = P.ProductPrice,
                                  ProductDetails = P.ProductDetail,
                                  ProductCompany = P.ProductCompany,
                                  Quantity = P.Quantity,
                                  ProductImageUrl = P.ProductImageUrl,
                                  PublicID = P.PublicId,
                                  IsArchive = P.IsArchive,
                                  IsActive = P.IsActive
                              })
              .OrderByDescending(X => X.InsertionDate)
              .Skip(request.NumberOfRecordPerPage * (request.PageNumber - 1))
              .Take(request.NumberOfRecordPerPage)
              .ToList();

                if (Result.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "WishList Not Found";
                    return response;
                }

                response.data = new List<GetAllWishListProduct>();
                foreach (var data in Result)
                {
                    GetAllWishListProduct Cartdata = new GetAllWishListProduct();

                    Cartdata.WishListID = data.WishListID;
                    Cartdata.InsertionDate = Convert.ToDateTime(data.InsertionDate).ToString("dddd, dd-MM-yyyy, HH:mm tt");
                    Cartdata.IsActive = data.IsActive;
                    Cartdata.IsArchive = data.IsArchive;
                    Cartdata.ProductCompany = data.ProductCompany;
                    Cartdata.ProductDetails = data.ProductDetails;
                    Cartdata.ProductID = data.ProductID;
                    Cartdata.ProductImageUrl = data.ProductImageUrl;
                    Cartdata.ProductName = data.ProductName;
                    Cartdata.ProductPrice = data.ProductPrice;
                    Cartdata.ProductType = data.ProductType;
                    Cartdata.PublicID = data.PublicID;
                    Cartdata.Quantity = data.Quantity;
                    response.data.Add(Cartdata);
                }

                response.TotalRecords = _dbContext.WishListDetails.Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;

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

        public async Task<MoveToCardResponse> MoveToCard(MoveToCardRequest request)
        {
            MoveToCardResponse response = new MoveToCardResponse();
            response.Message = "Add To Cart Successful";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"MoveToCard In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"INSERT INTO CardDetails (UserId, ProductID) VALUES (@UserId, @ProductID);
                                    DELETE FROM WishListDetails WHERE WishListID=@WishListID;
                ";

                CardDetails cardDetails = new CardDetails()
                {
                    UserId=request.UserID, 
                    ProductID=request.ProductID
                };

                await _dbContext.AddAsync(cardDetails);
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                    return response;
                }

                var WishlistDetails = _dbContext.WishListDetails
                                                       .Where(X => X.WishListID == request.WishListID)
                                                       .FirstOrDefault();
                
                if(WishlistDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Wish List ID Not Found";
                }

                 _dbContext.WishListDetails.Remove(WishlistDetails);
                Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
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

        public async Task<RemoveWishListProductResponse> RemoveWishListProduct(RemoveWishListProductRequest request)
        {
            RemoveWishListProductResponse response = new RemoveWishListProductResponse();
            response.Message = "Remove WishList Product Successfully";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"RemoveWishListProduct In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"DELETE FROM WishListDetails WHERE WishListID=@WishListID";

                var WishlistDetails = _dbContext.WishListDetails
                                                        .Where(X=>X.WishListID==request.WishListID)
                                                        .FirstOrDefault();
                if (WishlistDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Wish List ID Not Found";
                }

                _dbContext.WishListDetails.Remove(WishlistDetails);
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went wrong";
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
