using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class UpdateProductRequest
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public string ProductPrice { get; set; }
        public string ProductDetails { get; set; }
        public string ProductCompany { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value Greater than 0")]
        public int Quantity { get; set; }
    }

    public class UpdateProductResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
