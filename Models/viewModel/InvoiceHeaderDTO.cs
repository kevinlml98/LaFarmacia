using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class InvoiceHeaderDTO
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int PayMethodType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}