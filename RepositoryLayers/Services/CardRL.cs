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

using System.IO;
using System.Net;
using System.Net.Mail;

namespace RepositoryLayers.Services
{
    public class CardRL : ICardRL
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _SqlConnection;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CardRL> _logger;
        public CardRL(IConfiguration configuration, ApplicationDbContext dbContext, ILogger<CardRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _SqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
        }

        public async Task<AddRatingResponse> AddRating(AddRatingRequest request)
        {
            AddRatingResponse response = new AddRatingResponse();
            response.IsSuccess = true;
            response.Message = "Add Rating Successfully";

            try
            {
                _logger.LogInformation($"AddRating Calling In CardRL....{JsonConvert.SerializeObject(request)}");
                
                var CartDetails = _dbContext.CardDetails
                                        .Where(X => X.CardID == request.CartID)
                                        .FirstOrDefault();

                if (CartDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart ID Not Found";
                    return response;
                }

                CartDetails.Rating = request.Rating;
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }

            return response;
        }

        public async Task<AddToCardResponse> AddToCard(AddToCardRequest request)
        {
            AddToCardResponse response = new AddToCardResponse();
            response.Message = "Add To Cart Successfully.";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"AddToCard Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                //string SqlQuery = @"INSERT INTO CardDetails (UserId, ProductID) VALUES (@UserId, @ProductID)";

                CardDetails cardDetails = new CardDetails()
                {
                    UserId = request.UserID,
                    ProductID = request.ProductID
                };

                await _dbContext.AddAsync(cardDetails);
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
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<OrderProductResponse> CancleOrder(OrderProductRequest request)
        {
            OrderProductResponse response = new OrderProductResponse();
            response.Message = "Successfully Cancle Your Order";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"CancleOrder Calling In CardRL....{JsonConvert.SerializeObject(request)}");
                /*string SqlQuery = @"  UPDATE ProductDetails
                                      SET Quantity=Quantity+1
                                      WHERE ProductID=@ProductID;
                                        
                                      Update CardDetails 
                                      SET IsOrder=0
                                       WHERE CardID=@CartID";*/

                var ProductDetails = await _dbContext.ProductDetails.FindAsync(request.ProductID);

                if (ProductDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product ID Not Found";
                    return response;
                }

                ProductDetails.Quantity = ProductDetails.Quantity + 1;
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

                var CartDetails = await _dbContext.CardDetails.FindAsync(request.CartID);

                if (CartDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart ID Not Found";
                    return response;
                }

                _dbContext.CardDetails.Remove(CartDetails);
                int DeleteResult = await _dbContext.SaveChangesAsync();
                if (DeleteResult <= 0)
                {
                    response.Message = "Something Went Wrong";
                }

                

                /*CartDetails.IsOrder = false;
                Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }*/


            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        // Inner Joins
        public async Task<GetAllCardDetailsResponse> GetAllCardDetails(GetAllCardDetailsRequest request)
        {
            GetAllCardDetailsResponse response = new GetAllCardDetailsResponse();
            response.Message = "Fetch All Carts Successfully";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetAllCardDetails Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                /*string SqlQuery = @"
                                    SELECT Distinct C.CardID, P.ProductID, 
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
                                           (SELECT COUNT(*) FROM CardDetails WHERE IsOrder=0 AND UserID=@UserID) AS TotalRecord
                                    FROM CardDetails C 
                                    INNER JOIN ProductDetails P
                                    On C.ProductID = P.ProductID
                                    WHERE IsOrder=0 AND UserID=@UserID
                                    ORDER BY P.InsertionDate DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;";*/

                var Result = (from c in _dbContext.CardDetails
                              join P in _dbContext.ProductDetails
                              on c.ProductID equals P.ProductID
                              where c.IsOrder == false && c.UserId == request.UserID
                              select new
                              {
                                  CardID = c.CardID,
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
                    response.Message = "Carts Not Found";
                    return response;
                }

                response.data = new List<GetAllCardDetails>();
                foreach (var data in Result)
                {
                    var UserData = _dbContext.CustomerDetails
                                                        .Where(X => X.UserID == data.UserID)
                                                        .FirstOrDefault();

                    GetAllCardDetails Cartdata = new GetAllCardDetails();
                    Cartdata.CartID = data.CardID;
                    Cartdata.FullName = UserData != null ? String.IsNullOrEmpty(UserData.FullName) ? "" : UserData.FullName : "";
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

                response.TotalRecords = _dbContext.CardDetails
                                        .Where(X => X.IsOrder == false)
                                        .Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        // Inner Joins
        public async Task<GetOrderProductResponse> GetOrderProduct(GetOrderProductRequest request)
        {
            GetOrderProductResponse response = new GetOrderProductResponse();
            response.Message = "Fetch Order List Successfully";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"GetOrderProduct Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                string SqlQuery = string.Empty;

                if (request.UserID != -1)
                {
                    /*SqlQuery = @"
                                    SELECT Distinct C.CardID, (SELECT Distinct U.FullName FROM CustomerDetails U WHERE U.UserID = C.UserId) AS FullName, 
                                           P.ProductID, 
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
                                           (SELECT COUNT(*) FROM CardDetails WHERE IsOrder=1 AND UserID=@UserID) AS TotalRecord
                                    FROM CardDetails C 
                                    INNER JOIN ProductDetails P
                                    On C.ProductID = P.ProductID
                                    WHERE IsOrder=1 AND UserID=@UserID
                                    ORDER BY P.InsertionDate DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;";*/


                    var Result = (from c in _dbContext.CardDetails
                                  join P in _dbContext.ProductDetails
                                  on c.ProductID equals P.ProductID
                                  where c.IsOrder == true && c.UserId == request.UserID
                                  select new
                                  {
                                      CardID = c.CardID,
                                      UserID = c.UserId,
                                      IsPayment = c.IsPayment,
                                      Rating=c.Rating,
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
                        response.Message = "Order Not Found";
                        return response;
                    }

                    response.data = new List<GetAllCardDetails>();
                    foreach (var data in Result)
                    {
                        var UserData = _dbContext.CustomerDetails
                                                        .Where(X => X.UserID == data.UserID)
                                                        .FirstOrDefault();

                        GetAllCardDetails Cartdata = new GetAllCardDetails();
                        Cartdata.CartID = data.CardID;
                        Cartdata.IsPayment= data.IsPayment;
                        Cartdata.Rating = data.Rating;
                        Cartdata.FullName = UserData != null ? String.IsNullOrEmpty(UserData.FullName) ? "" : UserData.FullName : "";
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
                }
                else
                {
                    /*SqlQuery = @"
                                    SELECT Distinct C.CardID, (SELECT Distinct U.FullName FROM CustomerDetails U WHERE U.UserID = C.UserId) AS FullName,
                                           P.ProductID, 
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
                                           (SELECT COUNT(*) FROM CardDetails WHERE IsOrder=1) AS TotalRecord
                                    FROM CardDetails C 
                                    INNER JOIN ProductDetails P
                                    On C.ProductID = P.ProductID
                                    WHERE IsOrder=1
                                    ORDER BY P.InsertionDate DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;";*/

                    var Result = (from c in _dbContext.CardDetails
                                  join P in _dbContext.ProductDetails
                                  on c.ProductID equals P.ProductID
                                  where c.IsOrder == true
                                  select new
                                  {
                                      CardID = c.CardID,
                                      UserID = c.UserId,
                                      Rating = c.Rating,
                                      IsPayment = c.IsPayment,
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
                        response.Message = "Order Not Found";
                        return response;
                    }

                    response.data = new List<GetAllCardDetails>();
                    foreach (var data in Result)
                    {
                        var UserData = _dbContext.CustomerDetails
                                                        .Where(X => X.UserID == data.UserID)
                                                        .FirstOrDefault();

                        GetAllCardDetails Cartdata = new GetAllCardDetails();

                        Cartdata.CartID = data.CardID;
                        Cartdata.IsPayment = data.IsPayment;
                        Cartdata.Rating = data.Rating;
                        Cartdata.FullName = UserData != null ? String.IsNullOrEmpty(UserData.FullName) ? "" : UserData.FullName : "";
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
                }

                response.TotalRecords = _dbContext.CardDetails.Where(X => X.IsOrder == true).Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<OrderProductResponse> OrderProduct(OrderProductRequest request)
        {
            OrderProductResponse response = new OrderProductResponse();
            response.Message = "Successfully Order Your Product";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"OrderProduct Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                /*string SqlQuery = @"  UPDATE ProductDetails
                                      SET Quantity=Quantity-1
                                      WHERE ProductID=@ProductID;
                                        
                                      Update CardDetails 
                                      SET IsOrder=1 
                                       WHERE CardID=@CartID";*/

                var ProductDetails = await _dbContext.ProductDetails.FindAsync(request.ProductID);

                if (ProductDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product ID Not Found";
                    return response;
                }

                ProductDetails.Quantity = ProductDetails.Quantity - 1;
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

                var CartDetails = await _dbContext.CardDetails.FindAsync(request.CartID);

                if (CartDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart ID Not Found";
                    return response;
                }

                CartDetails.IsOrder = true;
                Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<RemoveCardResponse> RemoveCartProduct(RemoveCardRequest request)
        {
            RemoveCardResponse response = new RemoveCardResponse();
            response.Message = "Remove Cart Successfully.";
            response.IsSuccess = true;
            try
            {
                _logger.LogInformation($"RemoveCartProduct Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"DELETE FROM CardDetails WHERE CardID = @CartID";

                var CardDetails = await _dbContext.CardDetails.FindAsync(request.CartID);
                if (CardDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart Record Not Found";
                }

                _dbContext.CardDetails.Remove(CardDetails);
                int DeleteResult = await _dbContext.SaveChangesAsync();
                if (DeleteResult <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went Wrong";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                response.IsSuccess = false;
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }
    
        public async Task<PaymentGetwayResponse> PaymentGetway(PaymentGetwayRequest request)
        {
            PaymentGetwayResponse response = new PaymentGetwayResponse();
            response.IsSuccess = true;
            response.Message = "Send Payment Successfully";

            try
            {
                _logger.LogInformation($"PaymentGetway Calling In CardRL....{JsonConvert.SerializeObject(request)}");

                var CartDetails = _dbContext.CardDetails
                                        .Where(X => X.CardID == request.CartNo)
                                        .FirstOrDefault();

                if (CartDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart ID Not Found";
                    return response;
                }
                CartDetails.IsPayment = true;
                CartDetails.UpiId = request.Upiid;
                CartDetails.PaymentType = request.PaymentType;
                CartDetails.CardNo = request.CardNo;
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                    return response;
                }
                SendInvoiceMailRequest request1 = new SendInvoiceMailRequest();
                var ProductDetails = _dbContext.ProductDetails.Where(X => X.ProductID == CartDetails.ProductID).FirstOrDefault();
                request1.ProductImageUrl = ProductDetails.ProductImageUrl;
                request1.ProductId = ProductDetails.ProductID.ToString();
                request1.ProductPrice = ProductDetails.ProductPrice;
                request1.ProductName = ProductDetails.ProductName;
                request1.ProductDetails = ProductDetails.ProductDetail;
                request1.ProductCompany = ProductDetails.ProductCompany;
                request1.ProductType = ProductDetails.ProductType;

                var UserDetails = _dbContext.CustomerDetails.Where(X => X.UserID == CartDetails.UserId).FirstOrDefault();
                request1.CustomerId = UserDetails.EmailID;
                var responses = await SendInvoiceMail(request1);
                if (!responses.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong : "+response.Message;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }

            return response;
        }

        private async Task<SendInvoiceMailResponse> SendInvoiceMail(SendInvoiceMailRequest request)
        {
            SendInvoiceMailResponse response = new SendInvoiceMailResponse();
            response.IsSuccess = true;
            response.Message = "Send Invoice Mail Successfully";

            try
            {
                _logger.LogInformation($"SendInvoiceMail Calling In CardRL....{JsonConvert.SerializeObject(request)}");
                string to = request.CustomerId; //To address    
                string from = "1000thebeast1000@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "In this article you will learn how to send a email using Asp.Net & C#";
                message.Subject = "Grocery Product InVoice Mail.";
                message.Body = HtmlBody(request);
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("1000thebeast1000@gmail.com", "lgvmbgmzkwvhygjz");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "Exception Message : " + ex.Message;
                    _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }

            return response;
        }

        private string HtmlBody(SendInvoiceMailRequest request)
        {
            return @"<html>
                        <head>
                            <style>
                                #Container {
                                    height: 100%;
                                    width: 100%;
                                }

                                #SubContainer {
                                    height: 500px;
                                    width: 500px;
                                    border: 0.5px solid lightgray;
                                    padding: 10px 20px;
                                    font-family: Sans-serif;
                                    box-shadow: 0 0 8px -2px black;
                                }

                                #Header {
                                    font-size: 25px;
                                    font-weight: 525;
                                    color: blue;
                                    height: 10%;
                                }

                                #SubHeader {
                                    color: green;
                                    font-weight: 550;

                                }

                                #Body {
                                    height: 82%;
                                }

                                #ProductImage {
                                    height: 40%;
                                    width: 40%;
                                    margin: 15px 0;
                                }

                                .Product {
                                    margin: 10px 0;
                                }

                                #Footer {
                                    height: 8%;
                                    font-weight: 500;
                                    color: rgb(101, 101, 101);
                                    text-decoration: underline;
                                }
                            </style>
                        </head>

                        <body>
                            <div id='Container'>
                                <div id='SubContainer'>
                                    <div id='Header'>Glocery Store Invoice Mail</div>
                                    <div id='SubHeader'>Your Order Place Successfully.</div>
                                    <div id='Body'>
                                        <img id='ProductImage'
                                            src='" + request.ProductImageUrl+ @"' />
                                        <div class='Product'>Customer Id : "+request.CustomerId+ @" </div>
                                        <div class='Product'>Product Id : "+request.ProductId+ @" </div>
                                        <div class='Product'>Product Name : "+request.ProductName+ @" </div>
                                        <div class='Product'>Product Discription : "+request.ProductDetails+ @" </div>
                                        <div class='Product'>Product Price : "+request.ProductPrice+ @" </div>
                                        <div class='Product'>Product Type : "+request.ProductType+ @" </div>
                                        <div class='Product'>Product Company : "+request.ProductCompany+@" </div>
                                    </div>
                                    <div id='Footer'>If you not order any glocery product then please ignore this message.</div>
                                </div>
                            </div>
                        </body>
                   </html>";
        }

    }
}
