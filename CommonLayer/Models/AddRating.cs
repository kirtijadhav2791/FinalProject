using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddRatingRequest
    {
        public int CartID { get; set; }
        public int Rating { get; set; }
    }

    public class AddRatingResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class RatingCalculation
    {
        public double TotalRating { get; set; }
        public int TotalVoter { get; set; }
        public List<UserCount> data { get; set; }
    }

    public class UserCount
    {
        public int UserID { get; set; }
        public double Rating { get; set; }
    } 
}
