using Company.Enums;
using Company.Models;
using Company.Services;
using Company.Utilities;
using Company.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    public class EmployeeAction
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ProjectRepository _projectRepository;
        public EmployeeAction(EmployeeRepository employeeService, DepartmentRepository departmentService, ProjectRepository projectService)
        {
            _employeeRepository = employeeService;
            _departmentRepository = departmentService;
            _projectRepository = projectService;
        }

        public void AddEmployee()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;
            int gender = ConsoleInput.Read<int>("Enter gender (0: Male, 1: Female): ")!;
            DateOnly birthDate = ConsoleInput.Read<DateOnly>("Enter birth date: ")!;

            Console.WriteLine("Department: ");
            var department = ConsoleInput.OptionalSelect(_departmentRepository.FindAllDepartments());


            Console.WriteLine("Projects: ");
            var projects = ConsoleInput.OptionalSelectMultiple(_projectRepository.FindAllProjects());

            var employee = new Employee
            {
                Name = Name,
                Gender = (Gender) gender,
                BirthDate = birthDate,
                Department = department,
                Projects = projects
            };

            ObjectValidator.ValidateObject(employee)
                .Then(() => _employeeRepository.SaveEmployee(employee))
                .ElseDisplayErrors("Invalid employee data: ");
        }

        public void EditEmployee()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;
            var storedEmployee = _employeeRepository.FindEmployeeByName(Name);
            if (storedEmployee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            var EditPropertyMenu = new SubMenuItem("Edit Property", new List<MenuItem>
            {
                new ActionMenuItem("Name", () => storedEmployee.Name = ConsoleInput.Read<string>("Enter new name: ")!),
                new ActionMenuItem("Gender", () => storedEmployee.Gender = (Gender)ConsoleInput.Read<int>("Enter new gender (0: Male, 1: Female): ")),
                new ActionMenuItem("Birth Date", () => storedEmployee.BirthDate = ConsoleInput.Read<DateOnly>("Enter new birth date: ")),
                new ActionMenuItem("Assign to Department", () => AssignToDepartment(storedEmployee)),
                new ActionMenuItem("Remove from Department", () => storedEmployee.Department = null),
                new ActionMenuItem("Assign to Project", () =>  AssignToProject(storedEmployee)),
                new ActionMenuItem("Remove from Project", () => RemoveFromProject(storedEmployee))
            }, 
            () => ListEmployee(storedEmployee));
            EditPropertyMenu.Execute(clearBefore: false);

            ObjectValidator.ValidateObject(storedEmployee)
                .Then(() => _employeeRepository.UpdateEmployee(storedEmployee))
                .ElseDisplayErrors("Invalid employee data: ");
        }
        public void DeleteEmployee() {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;
            var storedEmployee = _employeeRepository.FindEmployeeByName(Name);
            if (storedEmployee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            _employeeRepository.DeleteEmployee(storedEmployee);
        }

        public void ListEmployees()
        {
            var employees = _employeeRepository.FindAllEmployees();
            foreach (var employee in employees)
            {
                ListEmployee(employee);
            }
        }

        private void ListEmployee(Employee employee)
        {
            Console.WriteLine(employee);
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Projects: ");
            employee.Projects?.ForEach(project => Console.WriteLine($"\t {project}"));
            Console.WriteLine("-----------------------------");
        }
        private void AssignToDepartment(Employee storedEmployee)
        {
            var department = ConsoleInput.OptionalSelect(_departmentRepository.FindAllDepartments());
            if (department is not null)
            {
                storedEmployee.Department = department;
            }
        }
        private void AssignToProject(Employee storedEmployee)
        {
            storedEmployee.Projects = storedEmployee.Projects ?? new List<Project>();
            var selectedProject = ConsoleInput.OptionalSelect(_projectRepository.FindAllProjects());
            if (selectedProject is not null)
            {
                storedEmployee.Projects.Add(selectedProject);
            }
        }

        private void RemoveFromProject(Employee storedEmployee)
        {
            if (storedEmployee.Projects == null || storedEmployee.Projects.Count == 0)
            {
                Console.WriteLine("No projects assigned to this employee.");
                return;
            }
            storedEmployee.Projects.Remove(ConsoleInput.Select(storedEmployee.Projects.ToList()));
        }
    }
}
