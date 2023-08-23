using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class ProductMoveToArchiveRequest
    {
        [Required]
        public int ProductID { get; set; }
    }

    public class ProductMoveToArchiveResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
