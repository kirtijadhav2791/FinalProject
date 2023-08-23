using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllProductRequest
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int NumberOfRecordPerPage { get; set; }
    }

    public class GetAllProductResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; }
        public double TotalRecords { get; set; }
        public int TotalPage { get; set; }
        public List<GetAllProduct> data { get; set; }
    }

    public class GetAllProduct
    {
        //ProductID, InsertionDate, ProductName,ProductType
        //ProductPrice, ProductDetails, ProductCompany, Quantity, IsArchive, IsActive
        public int ProductID { get; set; }
        public string InsertionDate { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDetails { get; set; }
        public string ProductCompany { get; set; }
        public int Quantity { get; set; }
        public string ProductImageUrl { get; set; }
        public string PublicID { get; set; }
        public double Rating { get; set; }
        public bool IsArchive { get; set; }
        public bool IsActive { get; set; }
       
    }

    public class GetProductByIDRequest
    {
        [Required]
        public int ProductID { get; set; }
    }

    public class GetProductByIDResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public GetAllProduct data { get; set; }
    }

    public class GetProductByNameRequest
    {
        [Required]
        public string ProductName { get; set; }
    }

    public class GetProductByNameResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public GetAllProduct data { get; set; }
    }
}
