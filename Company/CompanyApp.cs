using Company.Data;
using Company.Services;
using Company.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company
{
    public class CompanyApp
    {
        private readonly SubMenuItem _menu;

        public CompanyApp()
        {
            var context = new CompanyContext();

            var employeeRepository = new EmployeeRepository(context);
            var departmentRepository = new DepartmentRepository(context);
            var projectRepository = new ProjectRepository(context);

            var employeeAction = new EmployeeAction(
                employeeRepository,
                departmentRepository,
                projectRepository);

            var departmentAction = new DepartmentAction(
                departmentRepository,
                employeeRepository,
                projectRepository);

            var projectAction = new ProjectAction(
                projectRepository,
                employeeRepository,
                departmentRepository);


            _menu = new SubMenuItem("Main Menu", new List<MenuItem>
        {
            new SubMenuItem("Employee Management", new List<MenuItem>
            {
                new ActionMenuItem("Add", employeeAction.AddEmployee),
                new ActionMenuItem("Edit", employeeAction.EditEmployee),
                new ActionMenuItem("Delete", employeeAction.DeleteEmployee),
                new ActionMenuItem("Display", employeeAction.ListEmployees)
            }),

            new SubMenuItem("Department Management", new List<MenuItem>
            {
                new ActionMenuItem("Add", departmentAction.AddDepartment),
                new ActionMenuItem("Edit", departmentAction.EditDepartment),
                new ActionMenuItem("Delete", departmentAction.DeleteDepartment),
                new ActionMenuItem("List", departmentAction.ListDepartments)
            }),

            new SubMenuItem("Project Management", new List<MenuItem>
            {
                new ActionMenuItem("Add", projectAction.AddProject),
                new ActionMenuItem("Edit", projectAction.EditProject),
                new ActionMenuItem("Delete", projectAction.DeleteProject),
                new ActionMenuItem("List", projectAction.ListProjects)
            })
        });
        }

        public void Run()
        {
            _menu.Execute();
        }
    }
}
