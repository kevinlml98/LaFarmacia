using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public string RolDescription { get; set; }
        public bool State { get; set; }
        public string Password { get; set; }
    }
}