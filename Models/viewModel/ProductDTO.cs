using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class ProductDTO
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool State { get; set; }
        public string ProductTypeCode { get; set; }
    }
}