using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }
        [StringLength(1000, MinimumLength = 2)]
        public string? Description { get; set; }
        public List<Employee>? Employees { get; set; }

        public override string ToString()
        {
            return $"(Name= {Name}, Description= {Description})";
        }
    }
}
