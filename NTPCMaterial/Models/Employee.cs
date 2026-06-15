using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NTPCMaterial.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string PasswordHash { get; set; }
        [Required , MaxLength(20)]
        public string Role { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}