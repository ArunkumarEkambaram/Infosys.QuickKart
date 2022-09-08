using System;
using System.Collections.Generic;
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
        public decimal Price { get; set; }

        [JsonIgnore]
        public int QuantityAvailable { get; set; }

        [JsonIgnore] //Ignore the column
        public virtual Categories Category { get; set; }
    }
}
