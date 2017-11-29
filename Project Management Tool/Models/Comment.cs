using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime dateTime { get; set; }

        [Required]
        public string CommentBy { get; set; }

        public int TaskId { get; set; }
        
        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }
    }
}