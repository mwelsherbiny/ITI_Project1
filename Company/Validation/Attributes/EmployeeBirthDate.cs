using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Validation.Attributes
{
    public class EmployeeBirthDate : ValidationAttribute
    {
        public EmployeeBirthDate() { }
        override public bool IsValid(object? value)
        {
            if (value is DateOnly birthDate)
            {
                var age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now.Month < birthDate.Month 
                    || (DateTime.Now.Month == birthDate.Month && DateTime.Now.Day < birthDate.Day))
                {
                    age--;
                }
                return age >= 18 && age <= 70;
            }
            return false;
        }
    }
}
