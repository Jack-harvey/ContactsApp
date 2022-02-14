using System;
using System.ComponentModel.DataAnnotations;
namespace ContactsApp.Models.CompanyViewModels
{
    public class CompanyGroup
    {
        public string? CompanyName { get; set; }
        public int CompanyCount { get; set; }
    }
}
