using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Payroll
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Salary { get; set; }
    }
}
