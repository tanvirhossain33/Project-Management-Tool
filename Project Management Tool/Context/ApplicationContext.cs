using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Project_Management_Tool.Models;

namespace Project_Management_Tool.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserDesignation> UserDesignations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}