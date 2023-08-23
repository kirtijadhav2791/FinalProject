using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddCustomerDetailRequest
    {
        public bool IsUpdate { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
    }

    public class AddCustomerDetailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class GetCustomerDetailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public GetCustomerDetail data { get; set; }
    }

    public class GetCustomerDetail
    {
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
    }
}
