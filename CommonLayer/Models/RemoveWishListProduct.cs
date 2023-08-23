using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class RemoveWishListProductRequest
    {
        [Required]
        public int WishListID { get; set; }
    }

    public class RemoveWishListProductResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
