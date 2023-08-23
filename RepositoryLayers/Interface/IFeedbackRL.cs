using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Interface
{
    public interface IFeedbackRL
    {
        public Task<GetFeedbacksResponse> GetFeedbacks(GetFeedbacksRequest request);
        public Task<AddFeedbackResponse> AddFeedback(AddFeedbackRequest request);
        public Task<DeleteFeedbackResponse> DeleteFeedback(int ID);
    }
}
