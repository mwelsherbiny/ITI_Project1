using Company.Enums;
using Company.Models;
using Company.Services;
using Company.Utilities;
using Company.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    internal class DepartmentAction
    {
        private DepartmentRepository _departmentRepository;
        private EmployeeRepository _employeeRepository;
        private ProjectRepository _projectRepository;

        public DepartmentAction(DepartmentRepository departmentRepository, EmployeeRepository employeeRepository, ProjectRepository projectRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
        }

        public void AddDepartment()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;

            Console.WriteLine("Employees: ");
            var employees = ConsoleInput.OptionalSelectMultiple(_employeeRepository.FindAllEmployees());

            var department = new Department
            {
                Name = Name,
                Employees = employees
            };

            ObjectValidator.ValidateObject(department)
                .Then(() => _departmentRepository.SaveDepartment(department))
                .ElseDisplayErrors("Invalid department data: ");
        }

        public void DeleteDepartment()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;
            var storedDepartment = _departmentRepository.FindDepartmentByName(Name);
            if (storedDepartment == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            _departmentRepository.DeleteDepartment(storedDepartment);
        }

        public void EditDepartment()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;

            var storedDepartment = _departmentRepository.FindDepartmentByName(Name);
            if (storedDepartment == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            var EditPropertyMenu = new SubMenuItem("Edit Property", new List<MenuItem>
            {
                new ActionMenuItem("Name", () => storedDepartment.Name = ConsoleInput.Read<string>("Enter new name: ")),
                new ActionMenuItem("Assign New Employee", () => AssignEmployeeToDepartment(storedDepartment)),
                new ActionMenuItem("Remove Employee", () => RemoveEmployeeFromDepartment(storedDepartment))
            },
            () => ListDepartment(storedDepartment));
            EditPropertyMenu.Execute(clearBefore: false);

            ObjectValidator.ValidateObject(storedDepartment)
                .Then(() => _departmentRepository.UpdateDepartment(storedDepartment))
                .ElseDisplayErrors("Invalid department data: ");
        }

        public void ListDepartments()
        {
            var departments = _departmentRepository.FindAllDepartments();
            foreach (var department in departments)
            {
                ListDepartment(department);
            }
        }

        private void ListDepartment(Department department)
        {
            Console.WriteLine(department);
            Console.WriteLine($"Employees: ");
            department.Employees?.ForEach(e => Console.WriteLine($"\t {e}"));
            Console.WriteLine("-----------------------------");
        }

        private void AssignEmployeeToDepartment(Department storedDepartment)
        {
            var newEmployee = ConsoleInput.OptionalSelect(_employeeRepository.FindAllEmployees());
            if (newEmployee == null)
            {
                return;
            }

            storedDepartment.Employees ??= new List<Employee>();
            if (storedDepartment.Employees.Any(e => e.Id == newEmployee.Id))
            {
                Console.WriteLine("Employee already exists in the department.");
                return;
            }
            storedDepartment.Employees.Add(newEmployee);
        }

        private void RemoveEmployeeFromDepartment(Department storedDepartment)
        {
            var employee = ConsoleInput.OptionalSelect(storedDepartment.Employees ?? new List<Employee>());
            if (employee == null
                || storedDepartment.Employees == null 
                || storedDepartment.Employees.Count == 0)
            {
                return;
            }

            var storedEmployee = storedDepartment.Employees.Find(e => e.Id == employee.Id);
            if (storedEmployee != null)
            {
                storedDepartment.Employees.Remove(storedEmployee);
            }
        }
    }
}
