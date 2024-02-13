using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adminpanel.Models
{
    public class AboutUsDetails
    {
        [Key]
        public int Id { get; set; }
        public string AboutUsHeading { get; set; }
        public string AboutUsDescreption { get; set; }
        public string AboutUsImage { get; set; }
        public string AboutUsPath { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
    }
}