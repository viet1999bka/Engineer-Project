using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class CodeFriend
    {
        public int Cfid { get; set; }
        public int? UserId { get; set; }
        public int? FriendId { get; set; }
        public int? Status { get; set; }
        public DateTime? DateJoin { get; set; }

        public virtual Account Friend { get; set; }
        public virtual Account User { get; set; }
    }
}
