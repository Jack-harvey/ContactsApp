using System;
using System.Collections.Generic;

namespace ContactsApp.TempModels
{
    public partial class Contact
    {
        public Guid ContactId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string? Company { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Picture { get; set; }
        public string? Notes { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
