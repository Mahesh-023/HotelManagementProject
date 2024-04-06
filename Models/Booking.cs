//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManagementProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public System.DateTime CheckInDate { get; set; }
        public System.DateTime CheckOutDate { get; set; }
        public Nullable<int> NoOfMember { get; set; }
        public decimal TotalAmount { get; set; }
    
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Customer Customer { get; set; }
    }
}