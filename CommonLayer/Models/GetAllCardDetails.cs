using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllCardDetailsRequest
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int NumberOfRecordPerPage { get; set; }

        [Required]
        public int UserID { get; set; }
    }

    public class GetAllCardDetailsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; }
        public double TotalRecords { get; set; }
        public int TotalPage { get; set; }
        public List<GetAllCardDetails> data { get; set; }
    }

    public class GetAllCardDetails
    {
        //ProductID, InsertionDate, ProductName,ProductType
        //ProductPrice, ProductDetails, ProductCompany, Quantity, IsArchive, IsActive
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public bool IsPayment { get; set; }
        public string InsertionDate { get; set; }
        public string FullName { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDetails { get; set; }
        public string ProductCompany { get; set; }
        public int Quantity { get; set; }
        public string ProductImageUrl { get; set; }
        public string PublicID { get; set; }
        public bool IsArchive { get; set; }
        public bool IsActive { get; set; }
        public double Rating { get; set; } 
    }
}
