using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adminpanel.Models
{
    public class CourseDetails
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CoursePunchLine { get; set; }
        public string Descreption { get; set; }
        public string CourseFileName { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }



    }
}