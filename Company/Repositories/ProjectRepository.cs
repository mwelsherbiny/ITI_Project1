using Company.Data;
using Company.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Services
{
    public class ProjectRepository
    {
        private readonly CompanyContext _context;

        public ProjectRepository(CompanyContext context)
        {
            _context = context;
        }
        public Project? FindProjectByName(string name)
        {
            return _context.Projects
                .Include(p => p.Employees)
                .FirstOrDefault(p => EF.Functions.Like(p.Name, $"%{name}%"));
        }
        public List<Project> FindAllProjects()
        {
            return _context.Projects
                .Include(p => p.Employees)
                .ToList();
        }
        public void SaveProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }
        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }
        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }
}
