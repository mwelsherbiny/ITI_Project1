using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Models
{
    public class Department
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 50 characters.")]
        public required string Name { get; set; }
        public List<Employee>? Employees { get; set; }

        public override string ToString()
        {
            return $"(Name= {Name})";
        }
    }
}
