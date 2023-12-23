using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaFarmacia.Models.viewModel
{
    public class Respuesta
    {

        public bool Resultado { get; set; }
        public string Texto { get; set; }

        public int Valor { get; set; }

        public List<T_Product> ListaDetalles { get; set; }

    }
}