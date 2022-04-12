using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Models
{
    //[Keyless]
    public partial class User
    {
        [Column("userId")]
        [Key]
        public int UserId { get; set; }
        [Column("userName")]
        [StringLength(255)]
        public string UserName { get; set; } = null!;
        [Column("themeSelection")]
        [StringLength(255)]
        public string? ThemeSelection { get; set; }
    }
}
