using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Models
{
    public partial class CompanyOffice
    {
        [Key]
        public Guid OfficeId { get; set; }
        public Guid CompanyId { get; set; }
        [StringLength(100)]
        public string Address { get; set; } = null!;
        [StringLength(50)]
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string PostCode { get; set; } = null!;

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("CompanyOffices")]
        public virtual Company? Company { get; set; } //= null!;
    }
}
