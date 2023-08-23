using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddToCardRequest
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int UserID { get; set; }
    }

    public class AddToCardResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class PaymentGetwayRequest
    {
        public int CartNo { get; set; }
        public string Upiid { get; set; }
        public string PaymentType { get; set; }
        public string CardNo { get; set; }
    }

    public class PaymentGetwayResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class SendInvoiceMailRequest
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDetails { get; set; }
        public string ProductCompany { get; set; }
        public string ProductImageUrl { get; set; }
    }

    public class SendInvoiceMailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
