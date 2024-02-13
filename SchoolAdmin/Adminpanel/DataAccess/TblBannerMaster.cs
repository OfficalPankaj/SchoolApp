using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Adminpanel.DataAccess
{
    public partial class TblBannerMaster
    {
        public int Id { get; set; }
        public string BannerHeading { get; set; }
        public string BannerImage { get; set; }
        public string BannerPath { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
