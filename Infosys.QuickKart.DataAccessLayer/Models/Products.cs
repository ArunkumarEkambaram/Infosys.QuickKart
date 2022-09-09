using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Infosys.QuickKart.DataAccessLayer.Models
{
    public partial class Products
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public byte? CategoryId { get; set; }

        [Range(minimum: 1000, maximum: 10000000, ErrorMessage = "Price must be between 1000 and 10000000")]
        public decimal Price { get; set; }

        [Range(minimum: 0, maximum: 200, ErrorMessage = "Quantity Available cannot exceed 200")]
        public int QuantityAvailable { get; set; }

        [JsonIgnore] //Ignore the column
        public virtual Categories Category { get; set; }
    }
}
