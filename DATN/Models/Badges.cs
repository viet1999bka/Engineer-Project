// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class Badges
    {
        public int BadgesId { get; set; }
        public string Name { get; set; }
        public byte[] Img { get; set; }
        public string Description { get; set; }
    }
}
