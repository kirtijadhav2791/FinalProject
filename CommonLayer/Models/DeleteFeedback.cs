using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class DeleteFeedbackRequest
    {
        [Required]
        public int FeedbackID { get; set; }
    }

    public class DeleteFeedbackResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
