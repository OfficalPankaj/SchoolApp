using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adminpanel.Models
{
    public class GalleryDetail
    {
        [Key]
        public int Id { get; set; }
        public string GalleryPunchLine { get; set; }
        public string GalleryFileName { get; set; }
        public string GalleryFilePath { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
    }
}