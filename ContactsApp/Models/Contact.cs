using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Models
{
    public partial class Contact
    {
        public Guid ContactId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Firstname { get; set; } = null!;
        [Required(AllowEmptyStrings = false)]
        public string Lastname { get; set; } = null!;
        public string? Company { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Picture { get; set; }
        public string? Notes { get; set; }
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
