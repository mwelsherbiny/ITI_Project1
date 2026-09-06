using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Validation
{
    public static class ObjectValidator
    {
        public static List<ValidationResult> ValidateObject(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(obj, context, results, true);
            return results;
        }

        public static void DisplayValidationResults(List<ValidationResult> results, string errorHeader = "Validation failed: ")
        {
            if (results.Count == 0)
            {
                return;
            }

            Console.WriteLine(errorHeader);
            results.ForEach(result => Console.WriteLine($"\t- {result.ErrorMessage}"));
        }
    }
}
