using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ContactsApp.Models
{
    public partial class Contact
    {
        [Key]
        public Guid ContactId { get; set; }
        [StringLength(50)]
        public string Firstname { get; set; } = null!;
        [StringLength(50)]
        public string Lastname { get; set; } = null!;
        public Guid? CompanyId { get; set; }
        [StringLength(15)]
        public string? Mobile { get; set; }
        [StringLength(50)]
        public string? Phone { get; set; }
        [StringLength(75)]
        [EmailAddress]
        public string? Email { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }
        [StringLength(255)]
        public string? Picture { get; set; }
        [StringLength(300)]
        public string? Notes { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Contacts")]
        public virtual Category? Category { get; set; } //= null!;
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Contacts")]
        public virtual Company? Company { get; set; }
    }
}
