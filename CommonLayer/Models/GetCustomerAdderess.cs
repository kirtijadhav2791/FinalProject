using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetCustomerAdderessRequest
    {
        [Required]
        public int UserID { get; set; }
    }

    public class GetCustomerAdderessResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public GetCustomerAdderess data { get; set; }
    }

    public class GetCustomerAdderess
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Distict { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string pincode { get; set; }
    }
}
