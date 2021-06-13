using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage ="Name can't exceed 50 characters")]
        public string Name { get; set; }
        [Required]       
        public int Age { get; set; }
        [MaxLength(50, ErrorMessage ="Email can't exceed 50 characters")]
        public string Email { get; set; }

    }
}
