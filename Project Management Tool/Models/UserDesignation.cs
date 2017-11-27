using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class UserDesignation
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public virtual List<User> Users { get; set; }
    }
}