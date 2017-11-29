using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string AssignedByUser { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }


        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        public virtual List<Comment> Comments { get; set; }
    }
}