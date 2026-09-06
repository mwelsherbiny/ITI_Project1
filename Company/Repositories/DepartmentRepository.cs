using Company.Data;
using Company.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Services
{
    public class DepartmentRepository
    {
        private readonly CompanyContext _context;

        public DepartmentRepository(CompanyContext context)
        {
            _context = context;
        }
        public Department? FindDepartmentByName(string name)
        {
            return _context.Departments
                .FirstOrDefault(d => EF.Functions.Like(d.Name, $"%{name}%"));
        }
        public List<Department> FindAllDepartments()
        {
            return _context.Departments
                .Include(d => d.Employees)
                .ToList();
        }
        public void SaveDepartment(Department department)
        {
            _context.Departments.Add(department); 
            _context.SaveChanges();
        }
        public void UpdateDepartment(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }
        public void DeleteDepartment(Department department)
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }
    }
}
