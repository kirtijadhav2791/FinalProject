using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddCustomerAdderessRequest
    {
        public bool IsUpdate { get; set; } = false;
        [Required]
        public int UserID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Distict { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string pincode { get; set; }
    }

    public class AddCustomerAdderessResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class GetIsCustomerDetailsFoundResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsFound { get; set; }
    }


}
