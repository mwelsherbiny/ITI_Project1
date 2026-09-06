using Company.Enums;
using Company.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }
        public required Gender Gender {  get; set; }
        [EmployeeBirthDate(ErrorMessage = "Invalid age, must be between 18 and 70")]
        public DateOnly? BirthDate { get; set; }
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        public List<Project>? Projects { get; set; }

        public override string ToString()
        {
            return $"(Name= {Name}, Gender= {Gender}, Birth Date= {BirthDate})";
        }
    }
}
