using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Models
{
    public partial class Company
    {
        public Company()
        {
            CompanyOffices = new HashSet<CompanyOffice>();
            Contacts = new HashSet<Contact>();
        }

        [Key]
        public Guid CompanyId { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; } = null!;
        [Column("ABN")]
        [StringLength(50)]
        public string Abn { get; set; } = null!;
        [StringLength(100)]
        public string? Website { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FoundingDate { get; set; }

        [InverseProperty(nameof(CompanyOffice.Company))]
        public virtual ICollection<CompanyOffice> CompanyOffices { get; set; }
        [InverseProperty(nameof(Contact.Company))]
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
