using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.tempModel
{
    [Keyless]
    public partial class VwCompanyCount
    {
        [StringLength(50)]
        public string CompanyName { get; set; } = null!;
        public int? CountOfCompanies { get; set; }
    }
}
