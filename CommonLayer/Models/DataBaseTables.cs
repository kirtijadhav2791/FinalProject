using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddressDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public DateTime UpdationDate { get; set; }
        public int UserID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Distict { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string pincode { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CardDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardID { get; set; }
        public int UserId { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public int ProductID { get; set; }
        public bool IsOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPayment { get; set; }
        public double Rating { get; set; }
        public string UpiId { get; set; }
        public string PaymentType { get; set; }
        public string CardNo { get; set; }
    }

    public class CustomerDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public DateTime UpdationDate { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class FeedbackDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public int UserID { get; set; }
        public string Feedback { get; set; }
    }

    public class ProductDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDetail { get; set; }
        public string ProductCompany { get; set; }
        public string ProductImageUrl { get; set; }
        public string PublicId { get; set; }
        public int Quantity { get; set; }
        public string Rating { get; set; }
        public bool IsArchive { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    public class UserDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Role { get; set; } // 'customer', 'admin', 'master'
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class WishListDetails {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishListID { get; set; }
        public int UserId { get; set; }
        public DateTime InsertionDate { get; set; } = DateTime.Now;
        public int ProductID {get; set;}
        public bool IsActive { get; set; } = true;
        }

}
