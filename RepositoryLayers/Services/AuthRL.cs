using CommonLayer;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayers.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Services
{
    public class AuthRL : IAuthRL
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _SqlConnection;
        private readonly ILogger<AuthRL> _logger;
        private readonly ApplicationDbContext _dbContext;
        public AuthRL(IConfiguration configuration, ILogger<AuthRL> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _SqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
            _dbContext = dbContext;
        }

        public async Task<AddCustomerAdderessResponse> AddCustomerAdderess(AddCustomerAdderessRequest request)
        {
            AddCustomerAdderessResponse response = new AddCustomerAdderessResponse();
            response.IsSuccess = true;
            int Result = 0;
            try
            {
                _logger.LogInformation($"AddCustomerAdderess Calling In AuthRL....{JsonConvert.SerializeObject(request)}");
                var UserDetails =  _dbContext.AddressDetails
                    .Where(X=>X.UserID==request.UserID)
                    .FirstOrDefault();

                if (UserDetails == null)
                {
                    response.Message = "Add Customer Address Successful";
                    AddressDetails addressDetails = new AddressDetails()
                    {
                        UserID = request.UserID,
                        Address1 = request.Address1,
                        Address2 = request.Address2,
                        City = request.City,
                        Distict = request.Distict,
                        State = request.State,
                        Country = request.Country,
                        pincode = request.pincode
                    };
                    await _dbContext.AddAsync(addressDetails);
                    Result = await _dbContext.SaveChangesAsync();
                    if (Result <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something Went Wrong";
                    }

                }
                else
                {
                    response.Message = "Update Customer Address Successful";
                    UserDetails.Address1 = request.Address1;
                    UserDetails.Address2 = request.Address2;
                    UserDetails.City = request.City;
                    UserDetails.Distict = request.Distict;
                    UserDetails.State = request.State;
                    UserDetails.Country = request.Country;
                    Result = await _dbContext.SaveChangesAsync();
                    if (Result <= 0)
                    {
                        //response.IsSuccess = false;
                        //response.Message = "Something Went Wrong";
                    }
                }
               

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<AddCustomerDetailResponse> AddCustomerDetail(AddCustomerDetailRequest request)
        {
            AddCustomerDetailResponse response = new AddCustomerDetailResponse();
            response.IsSuccess = true;
            
            try
            {
                _logger.LogInformation($"AddCustomerAdderess Calling In AuthRL....{JsonConvert.SerializeObject(request)}");
                var CustomerDetails = _dbContext.CustomerDetails
                                                        .Where(X => X.UserID == request.UserID)
                                                        .FirstOrDefault();

                if (CustomerDetails == null)
                {
                    response.Message = "Add Customer Detail Successful";
                    CustomerDetails customerDetails = new CustomerDetails();
                    customerDetails.InsertionDate = DateTime.Now;
                    customerDetails.FullName = request.FullName;
                    customerDetails.EmailID = request.EmailID;
                    customerDetails.MobileNumber = request.MobileNumber;
                    customerDetails.UserName = request.UserName;
                    customerDetails.UserID = request.UserID;
                    await _dbContext.AddAsync(customerDetails);
                }
                else
                {
                    response.Message = "Update Customer Detail Successful";
                    CustomerDetails.UpdationDate = DateTime.Now;
                    CustomerDetails.FullName = request.FullName;
                    CustomerDetails.EmailID = request.EmailID;
                    CustomerDetails.MobileNumber = request.MobileNumber;
                    //await _dbContext.AddAsync(CustomerDetails);
                }

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
                response.Message = "Exception Occurs : Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        // Left Join
        public async Task<CustomerListResponse> CustomerList(CustomerListRequest request)
        {
            CustomerListResponse response = new CustomerListResponse();
            response.IsSuccess = true;
            response.Message = "Fetch Customer List Successfully";
            try
            {
                _logger.LogInformation($"CustomerList Calling In AuthRL....{JsonConvert.SerializeObject(request)}");
                string SqlQuery = @"SELECT C.ID,C.InsertionDate,C.FullName,C.EmailID,C.MobileNumber,C.IsActive U.UserID, U.UserName,
                                    (SELECT COUNT(*) FROM UserDetail WHERE Role='customer') AS TotalRecord
                                    FROM  UserDetail U
                                    left join CustomerDetails C
                                    on U.UserId = C.UserID
                                    Where U.Role='customer'
                                    ORDER BY ID DESC
                                   OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;";

                var Result = (from U in _dbContext.UserDetail
                              join C in _dbContext.CustomerDetails
                              on U.UserId equals C.UserID
                              into CustomerDetail
                              from m in CustomerDetail.DefaultIfEmpty()
                              where U.Role == "customer"
                              select new
                              {
                                  ID = m.ID != null ? m.ID : -1,
                                  InsertionDate = m.InsertionDate != null ? m.InsertionDate.ToString() : "",
                                  FullName = m.FullName != null ? m.FullName : "",
                                  EmailID = m.EmailID != null ? m.EmailID : "",
                                  MobileNumber = m.MobileNumber != null ? m.MobileNumber : "",
                                  IsActive = m.IsActive != null ? m.IsActive : false,
                                  UserID = U.UserId,
                                  UserName = U.UserName
                              }).OrderByDescending(X => X.ID)
                              .Skip(request.NumberOfRecordPerPage * (request.PageNumber - 1))
                              .Take(request.NumberOfRecordPerPage)
                              .ToList();

                if (Result.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Customers Not Found";
                    return response;
                }

                response.data = new List<CustomerList>();
                foreach (var data in Result)
                {
                    CustomerList CustomerData = new CustomerList();
                    CustomerData.EmailID = data.EmailID;
                    CustomerData.FullName = data.FullName;
                    CustomerData.ID = data.ID;
                    CustomerData.InsertionDate = String.IsNullOrEmpty(data.InsertionDate) ? "" : Convert.ToDateTime(data.InsertionDate).ToString("dddd, dd-MM-yyyy, HH:mm tt");
                    CustomerData.IsActive = data.IsActive;
                    CustomerData.MobileNumber = data.MobileNumber;
                    CustomerData.UserID = data.UserID;
                    CustomerData.UserName = data.UserName;
                    response.data.Add(CustomerData);
                }


                response.TotalRecords = _dbContext.UserDetail.Where(X => X.Role == "customer").Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetCustomerAdderessResponse> GetCustomerAdderess(int UserID)
        {
            GetCustomerAdderessResponse response = new GetCustomerAdderessResponse();
            response.IsSuccess = true;
            response.Message = "Get Customer Address Successful";
            try
            {
                _logger.LogInformation($"GetCustomerAdderess Calling In AuthRL....{JsonConvert.SerializeObject(UserID)}");
                string SqlQuery = @"SELECT Address1, Address2, City, Distict, State, Country, pincode 
                                   FROM AddressDetails 
                                    WHERE UserID=@UserID";

                var SearchResult = _dbContext.AddressDetails.Where(X => X.UserID == UserID).FirstOrDefault();
                if (SearchResult == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User Address not found";
                    return response;
                }

                response.data = new GetCustomerAdderess();
                response.data.Address1 = SearchResult.Address1;
                response.data.Address2 = SearchResult.Address2;
                response.data.City = SearchResult.City;
                response.data.Distict = SearchResult.Distict;
                response.data.State = SearchResult.State;
                response.data.Country = SearchResult.Country;
                response.data.pincode = SearchResult.pincode;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetCustomerDetailResponse> GetCustomerDetail(int UserID)
        {
            GetCustomerDetailResponse response = new GetCustomerDetailResponse();
            response.data = new GetCustomerDetail();
            response.IsSuccess = true;
            response.Message = "Get Customer Detail Successful";
            try
            {
                _logger.LogInformation($"GetCustomerDetail Calling In AuthRL....{JsonConvert.SerializeObject(UserID)}");
                
                string SqlQuery = @"SELECT FullName ,EmailID, MobileNumber 
                                   FROM CustomerDetails 
                                    WHERE UserID=@UserID";

                var SearchResult = _dbContext.CustomerDetails.Where(X => X.UserID == UserID).FirstOrDefault();
                if (SearchResult == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Customer Details Not Found";
                    return response;
                }

                response.data.FullName = SearchResult.FullName;
                response.data.EmailID = SearchResult.EmailID;
                response.data.MobileNumber = SearchResult.MobileNumber;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            SignInResponse response = new SignInResponse();
            response.IsSuccess = true;
            response.Message = "Login Successful";
            try
            {
                _logger.LogInformation($"SignIn In DataAccessLayer Calling .... {JsonConvert.SerializeObject(request)}");

                var SearchResult = _dbContext.UserDetail.Where(X => X.UserName == request.UserName &&
                                                                                        X.PassWord == request.Password &&
                                                                                        X.Role == request.Role.ToLower()).FirstOrDefault();

                if (SearchResult == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User Login Failed";
                }
                else
                {
                    response.data = new SignIn();
                    response.data.InsertionDate = Convert.ToString(SearchResult.InsertionDate);
                    response.data.UserId = SearchResult.UserId;
                    response.data.UserName = SearchResult.UserName;
                    response.data.Role = request.Role;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");

            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            response.IsSuccess = true;
            response.Message = "Sign Up Successful";
            try
            {
                _logger.LogInformation($"SignUp In DataAccessLayer Calling .... Request Body {JsonConvert.SerializeObject(request)}");

                var SearchResult = _dbContext.UserDetail.Where(X => X.UserName == request.UserName).FirstOrDefault();
                if (SearchResult != null)
                {
                    response.Message = "UserName Already Exist.";
                    return response;
                }

                if (request.Password != request.ConfigPassword)
                {
                    response.Message = "Password and ConfirmPassword Not Match";
                    return response;
                }

                if (request.Role.ToLower() == "admin")
                {
                    if (request.MasterPassword != "India@123")
                    {
                        response.Message = "InCorrect Master Password";
                        return response;
                    }
                }

                UserDetail RequestBody = new UserDetail()
                {
                    UserName = request.UserName,
                    Role = request.Role.ToLower(),
                    PassWord = request.Password
                };

                await _dbContext.AddAsync(RequestBody);
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
                response.Message = ex.Message;
                _logger.LogError($"Exception Occurs : Message 2 : {ex.Message}");
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetIsCustomerDetailsFoundResponse> GetIsCustomerDetailsFound(int UserID)
        {
            GetIsCustomerDetailsFoundResponse response = new GetIsCustomerDetailsFoundResponse();
            //response.data = new GetCustomerDetail();
            response.IsSuccess = true;
            response.Message = "Get Is Customer Details Found";
            response.IsFound = true;
            try
            {
                _logger.LogInformation($"GetIsCustomerDetailsFound Calling In AuthRL....{JsonConvert.SerializeObject(UserID)}");
                if (_SqlConnection != null && _SqlConnection.State != ConnectionState.Open)
                {
                    await _SqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT * 
                                    FROM CustomerDetails c
                                    Inner join AddressDetails a
                                    on c.UserID = a.UserID
                                    WHERE c.UserID=@UserID or a.UserID=@UserID";

                var Result = (from c in _dbContext.CustomerDetails
                              join P in _dbContext.AddressDetails
                              on c.UserID equals P.UserID
                              where c.UserID == UserID || c.UserID == UserID
                              select new
                              {
                                  CardID = c.ID,
                                  UserID = P.ID
                              }).Count();



                //var SearchResult = _dbContext.CustomerDetails.Where(X => X.UserID == UserID).FirstOrDefault();
                if (Result == 0)
                {
                    response.Message = "Customer Details Not Found. Please Enter Customer Details.";
                    response.IsFound = false;
                }
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 1 : {ex.Message}");
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
