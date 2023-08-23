using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllWishListDetailsRequest
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int NumberOfRecordPerPage { get; set; }
        [Required]
        public int UserID { get; set; }
    }

    public class GetAllWishListDetailsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; }
        public double TotalRecords { get; set; }
        public int TotalPage { get; set; }
        public List<GetAllWishListProduct> data { get; set; }
    }
    public class GetAllWishListProduct
    {
        //ProductID, InsertionDate, ProductName,ProductType
        //ProductPrice, ProductDetails, ProductCompany, Quantity, IsArchive, IsActive
        public int WishListID { get; set; }
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
        public bool IsArchive { get; set; }
        public bool IsActive { get; set; }
    }

}
