using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Validation
{
    public static class ValidationResultExtensions
    {
        public static List<ValidationResult> Then(
        this List<ValidationResult> results,
        Action action)
        {
            if (results.Count == 0)
            {
                action();
            }

            return results;
        }

        public static List<ValidationResult> ElseDisplayErrors(
            this List<ValidationResult> results,
            string errorHeader = "Validation failed: ")
        {
            if (results.Count == 0) { 
                return results;
            }
            Console.WriteLine(errorHeader);
            results.ForEach(result => Console.WriteLine($"\t- {result.ErrorMessage}"));
            return results;
        }
    }
}
