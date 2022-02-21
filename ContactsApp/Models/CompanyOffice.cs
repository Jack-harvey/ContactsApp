using System;
using System.Collections.Generic;

namespace ContactsApp.Models
{
    public partial class CompanyOffice
    {
        public Guid OfficeId { get; set; }
        public Guid CompanyId { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
    }
}
