using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Adminpanel.DataAccess
{
    public partial class TblAdminLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDeleted { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}
