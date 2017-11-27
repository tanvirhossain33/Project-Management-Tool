using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Status { get; set; }

        public int UserDesignationId { get; set; }

        [ForeignKey("UserDesignationId")]
        public virtual UserDesignation UserDesignation { get; set; }


        public virtual List<ProjectTeam> ProjectTeams { get; set; }
        public virtual List<Task> Tasks { get; set; }
    }
}