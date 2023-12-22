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
    using System.ComponentModel.DataAnnotations;

    public partial class T_InvoiceHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_InvoiceHeader()
        {
            this.T_InvoiceDetailList = new HashSet<T_InvoiceDetail>();
        }
        [Required]
        [Display(Name = "Código de Factura")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Fecha de compra")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }
        [Required]
        [Display(Name = "Identificación del cliente")]
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Método de pago")]
        public int PayMethodTypeId { get; set; }
        [Required]
        [Display(Name = "Subtotal de la factura")]
        public decimal SubTotal { get; set; }
        [Required]
        [Display(Name = "Impuestos")]
        public decimal Tax { get; set; }
        [Required]
        [Display(Name = "Descuento")]
        public decimal Discount { get; set; }
        [Required]
        [Display(Name = "Total")]
        public decimal Total { get; set; }
        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        public virtual T_Product T_Product { get; set; }
        public virtual T_InvoiceDetail T_InvoiceDetail { get; set; }
        public virtual T_Client T_Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_InvoiceDetail> T_InvoiceDetailList { get; set; }
        public virtual T_MethodPay T_MethodPay { get; set; }
    }
}