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
    
    public partial class T_MethodPay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_MethodPay()
        {
            this.T_InvoiceHeader = new HashSet<T_InvoiceHeader>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_InvoiceHeader> T_InvoiceHeader { get; set; }
    }
}