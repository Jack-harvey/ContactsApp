using System;
using System.Collections.Generic;

namespace ContactsApp.TempModels
{
    public partial class Company
    {
        public Company()
        {
            CompanyOffices = new HashSet<CompanyOffice>();
        }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Abn { get; set; } = null!;
        public string? Website { get; set; }
        public DateTime? FoundingDate { get; set; }

        public virtual ICollection<CompanyOffice> CompanyOffices { get; set; }
    }
}
