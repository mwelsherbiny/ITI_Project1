using Company.Data;
using Company.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Services
{
    public class EmployeeRepository
    {
        private readonly CompanyContext _context;

        public EmployeeRepository(CompanyContext context)
        {
            _context = context;
        }
        public Employee? FindEmployeeByName(string name)
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Projects)
                .FirstOrDefault(e => EF.Functions.Like(e.Name, $"%{name}%"));
        }
        public List<Employee> FindAllEmployees()
        {
            return _context
                .Employees
                .Include(e => e.Department)
                .Include(e => e.Projects)
                .ToList();
        }
        public void SaveEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }
        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
