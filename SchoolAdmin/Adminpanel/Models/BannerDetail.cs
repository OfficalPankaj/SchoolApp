using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adminpanel.Models
{
    public class BannerDetail
    {
        [Key]
        public int Id { get; set; }
        public string BannerHeading { get; set; }
        public string BannerImage { get; set; }
        public string BannerPath { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
    }
}