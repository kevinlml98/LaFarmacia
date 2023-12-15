using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class InvoiceDetailDTO
    {
        public long Id { get; set; }
        public string InvoiceHeaderCode { get; set; }
        public string ProductCode { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}