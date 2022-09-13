using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Account
    {
        public Account()
        {
            CodeFriendFriend = new HashSet<CodeFriend>();
            CodeFriendUser = new HashSet<CodeFriend>();
            Education = new HashSet<Education>();
            ExerciseAttemp = new HashSet<ExerciseAttemp>();
            Experience = new HashSet<Experience>();
        }

        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Salt { get; set; }
        public int? RoleId { get; set; }
        public bool? Lock { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<CodeFriend> CodeFriendFriend { get; set; }
        public virtual ICollection<CodeFriend> CodeFriendUser { get; set; }
        public virtual ICollection<Education> Education { get; set; }
        public virtual ICollection<ExerciseAttemp> ExerciseAttemp { get; set; }
        public virtual ICollection<Experience> Experience { get; set; }
    }
}
