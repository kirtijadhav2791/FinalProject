using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class RemoveCardRequest
    {
        /*[Required]
        public int ProductID { get; set; }
        [Required]
        public int UserID { get; set; }*/
        [Required]
        public int CartID { get; set; }
    }

    public class RemoveCardResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
