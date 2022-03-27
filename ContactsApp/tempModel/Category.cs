using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.tempModel
{
    public partial class Category
    {
        public Category()
        {
            Contacts = new HashSet<Contact>();
        }

        [Key]
        public int CategoryId { get; set; }
        [StringLength(50)]
        public string Description { get; set; } = null!;

        [InverseProperty(nameof(Contact.Category))]
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
