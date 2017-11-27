using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class Comments
    {
        public int Id { get; set; }

        [Required]
        public string Comment { get; set; }

        
        public int TaskId { get; set; }

        

        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }
    }
}