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
    public class FeedbackRL : IFeedbackRL
    {
        private readonly SqlConnection _SqlConnection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FeedbackRL> _logger;
        private readonly ApplicationDbContext _dbContext;
        public FeedbackRL(ILogger<FeedbackRL> logger, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
            _SqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
        }

        public async Task<GetFeedbacksResponse> GetFeedbacks(GetFeedbacksRequest request)
        {
            GetFeedbacksResponse response = new GetFeedbacksResponse();
            response.IsSuccess = true;
            response.Message = "Fetch All Feedback Successful";

            try
            {

                _logger.LogInformation($"GetFeedbacks In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                int Offset = (request.PageNumber - 1) * request.NumberOfRecordPerPage;
                string SqlQuery = @"SELECT * ,
                                    (select UserName from UserDetail Where UserID=F.UserID ) As UserName,
                                    (select COUNT(*) from FeedbackDetail) As TotalRecord
                                    FROM FeedbackDetail F
                                    ORDER BY FeedbackID DESC
                                    OFFSET @Offset ROWS FETCH NEXT @NumberOfRecordPerPage ROWS ONLY;
                                    ";

                var FeedbackResult = _dbContext.FeedbackDetail
                                                   .OrderByDescending(X => X.FeedbackID)
                                                   .Skip(Offset)
                              .Take(request.NumberOfRecordPerPage)
                              .ToList();

                if(FeedbackResult.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "No Feedback Found";
                    return response;
                }

                response.data = new List<GetFeedbacks>();
                foreach(var FeedbackDetail in FeedbackResult)
                {
                    // UserID Convert To UserName
                    var UserDetail = _dbContext.UserDetail
                                            .Where(X => X.UserId == FeedbackDetail.UserID)
                                            .FirstOrDefault();

                    if (UserDetail != null)
                    {
                        response.data.Add(new GetFeedbacks()
                        {
                            FeedbackID = FeedbackDetail.FeedbackID,
                            UserName = UserDetail.UserName,
                            FeedBack = FeedbackDetail.Feedback
                        });
                    }
                }

                response.TotalRecords = _dbContext.FeedbackDetail.Count();
                response.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.NumberOfRecordPerPage)));
                response.CurrentPage = request.PageNumber;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message 3 : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 3 : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<AddFeedbackResponse> AddFeedback(AddFeedbackRequest request)
        {
            AddFeedbackResponse response = new AddFeedbackResponse();
            response.IsSuccess = true;
            response.Message = "Send Feedback Successful";

            try
            {
                _logger.LogInformation($"AddFeedback In DataAccessLayer Calling .... request Body : {JsonConvert.SerializeObject(request)}");

                string SqlQuery = @"INSERT INTO FeedbackDetail (FeedBack, UserID)
                                    VALUES(@FeedBack, @UserID);";

                FeedbackDetail feedbackDetail = new FeedbackDetail()
                {
                    Feedback=request.Feedback, 
                    UserID=request.UserID
                };

                await _dbContext.AddAsync(feedbackDetail);
                var Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went Wrong";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message 2 : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 2 : ", ex.Message);
            }
            finally
            {
                await _SqlConnection.CloseAsync();
                await _SqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<DeleteFeedbackResponse> DeleteFeedback(int ID)
        {
            DeleteFeedbackResponse response = new DeleteFeedbackResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                _logger.LogInformation($"DeleteFeedback In DataAccessLayer Calling .... request ID : {ID}");

                string SqlQuery = @"DELETE FROM FeedbackDetail WHERE FeedbackID=@ID;";

                var Result = await _dbContext.FeedbackDetail.FindAsync(ID);
                if (Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Record Not Found";
                }

                _dbContext.FeedbackDetail.Remove(Result);
                int DeleteResult = await _dbContext.SaveChangesAsync();
                if (DeleteResult <= 0)
                {
                    response.Message = "Something went Wrong";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : Message 2 : " + ex.Message;
                _logger.LogError($"Exception Occurs : Message 2 : ", ex.Message);
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
