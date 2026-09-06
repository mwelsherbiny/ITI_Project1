using Company.Models;
using Company.Services;
using Company.Utilities;
using Company.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    internal class ProjectAction
    {
        private ProjectRepository _projectRepository;
        private EmployeeRepository _employeeRepository;
        private DepartmentRepository _departmentRepository;

        public ProjectAction(ProjectRepository projectRepository, EmployeeRepository employeeRepository, DepartmentRepository departmentRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public void AddProject()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ");
            string Description = ConsoleInput.Read<string>("Enter description: ");

            Console.WriteLine("Employees: ");
            var employees = ConsoleInput.OptionalSelectMultiple(_employeeRepository.FindAllEmployees());

            var project = new Project
            {
                Name = Name,
                Description = Description,
                Employees = employees
            };

            ObjectValidator.ValidateObject(project)
                .Then(() => _projectRepository.SaveProject(project))
                .ElseDisplayErrors("Invalid project data: ");
        }

        public void DeleteProject()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ");

            var storedProject  = _projectRepository.FindProjectByName(Name);
            if (storedProject == null)
            {
                Console.WriteLine("Project not found.");
                return;
            }

            _projectRepository.DeleteProject(storedProject);
        }

        public void EditProject()
        {
            string Name = ConsoleInput.Read<string>("Enter name: ")!;

            var storedProject = _projectRepository.FindProjectByName(Name);
            if (storedProject == null)
            {
                Console.WriteLine("Project not found.");
                return;
            }

            var EditPropertyMenu = new SubMenuItem("Edit Property", new List<MenuItem>
            {
                new ActionMenuItem("Name", () => storedProject.Name = ConsoleInput.Read<string>("Enter new name: ")!),
                new ActionMenuItem("Description", () => storedProject.Description = ConsoleInput.Read<string>("Enter new description: ")!),
                new ActionMenuItem("Assign New Employee", () => AssignEmployeeToProject(storedProject)),
                new ActionMenuItem("Remove Employee", () => RemoveEmployeeFromProject(storedProject)),
            }, 
            () => ListProject(storedProject));
            EditPropertyMenu.Execute(clearBefore: false);

            ObjectValidator.ValidateObject(storedProject)
                .Then(() => _projectRepository.UpdateProject(storedProject))
                .ElseDisplayErrors("Invalid project data: ");
        }
        public void ListProjects()
        {
            var projects = _projectRepository.FindAllProjects();
            foreach (var project in projects)
            {
                ListProject(project);
            }
        }

        private void ListProject(Project project)
        {
            Console.WriteLine(project);
            Console.WriteLine($"Employees: ");
            project.Employees?.ForEach(e => Console.WriteLine($"\t {e}"));
            Console.WriteLine("-----------------------------");
        }
        private void AssignEmployeeToProject(Project storedProject)
        {
            var newEmployee = ConsoleInput.OptionalSelect(_employeeRepository.FindAllEmployees());
            if (newEmployee == null)
            {
                return;
            }

            storedProject.Employees ??= new List<Employee>();
            if (storedProject.Employees.Any(e => e.Id == newEmployee.Id))
            {
                Console.WriteLine("Employee already exists in the project.");
                return;
            }
            storedProject.Employees.Add(newEmployee);
        }

        private void RemoveEmployeeFromProject(Project storedProject)
        {
            var employee = ConsoleInput.OptionalSelect(storedProject.Employees ?? new List<Employee>());
            if (employee == null
                || storedProject.Employees == null
                || storedProject.Employees.Count == 0)
            {
                return;
            }

            var storedEmployee = storedProject.Employees.Find(e => e.Id == employee.Id);
            if (storedEmployee != null)
            {
                storedProject.Employees.Remove(storedEmployee);
            }
        }
    }
}
