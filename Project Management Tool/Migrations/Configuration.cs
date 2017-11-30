namespace Project_Management_Tool.Migrations
{
    using Project_Management_Tool.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    
    internal sealed class Configuration : DbMigrationsConfiguration<Project_Management_Tool.Context.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project_Management_Tool.Context.ApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.UserDesignations.AddOrUpdate(
                new UserDesignation { Id = 1, Type = "IT Admin" },
                new UserDesignation { Id = 2, Type = "Project Manager" },
                new UserDesignation { Id = 3, Type = "Team Lead" },
                new UserDesignation { Id = 4, Type = "Developer" },
                new UserDesignation { Id = 5, Type = "QA Engineer" },
                new UserDesignation { Id = 6, Type = "UX Engineer" }
            );
            context.Users.AddOrUpdate(
                new User { Id = 1, Name = "Tanvir", Email = "tanvir@gmail.com", UserName = "tanvir@gmail.com", Password = "123", Status = "Active", UserDesignationId = 1 }
                );

        }
    }
}
