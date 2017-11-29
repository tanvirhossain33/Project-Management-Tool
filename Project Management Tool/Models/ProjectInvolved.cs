using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Management_Tool.Models
{
    public class ProjectInvolved
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Status { get; set; }
        public int NOM { get; set; }
        public int NOT { get; set; }

        public int UserId { get; set; }
    }
}