using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class ProjectTeam
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}