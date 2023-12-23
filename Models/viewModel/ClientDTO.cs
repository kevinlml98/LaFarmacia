using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel


{
    public class ClientDTO
    {


        [Required]
        [Display(Name = "Identificacion")]


        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Correo")]
        public string Email { get; set; }
    }
}