using System;
using System.Collections.Generic;

namespace ContactsApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Contacts = new HashSet<Contact>();
        }

        public int CategoryId { get; set; }
        public string Description { get; set; } = null!;


        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
