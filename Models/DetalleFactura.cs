//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaFarmacia.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetalleFactura
    {
        public int CodigoDetalleFactura { get; set; }
        public string CodigoFactura { get; set; }
        public string CodigoProductoComprado { get; set; }
        public int CantidadProductoComprado { get; set; }
        public Nullable<decimal> TotalPagoLinea { get; set; }
    
        public virtual Producto Producto { get; set; }
        public virtual EncabezadoFactura EncabezadoFactura { get; set; }
    }
}
