using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Management_Tool.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Remote("IsEmailAvailable", "Account", ErrorMessage = "Email is already exists.")]
        public string Email { get; set; }


        [Required]
        [EmailAddress]
        [NotMapped]
        [DisplayName("Email")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public String Status { get; set; }

        public int UserDesignationId { get; set; }

        [ForeignKey("UserDesignationId")]
        public virtual UserDesignation UserDesignation { get; set; }


        public virtual List<ProjectTeam> ProjectTeams { get; set; }
        public virtual List<Task> Tasks { get; set; }
    }
}