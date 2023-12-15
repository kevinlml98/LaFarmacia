using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string[] Perrmission { get; set; }
    }
}