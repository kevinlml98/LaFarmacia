using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LaFarmacia.Models.viewModel
{
    public class UserDTO
    {
        [Required]
        [Display(Name = "Identificacion")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre completo")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Correo electronnico")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Tipo usuario")]
        public int RolId { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string RolDescription { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public bool State { get; set; }
        [Required]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}