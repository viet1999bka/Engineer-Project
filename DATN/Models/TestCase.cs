// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class TestCase
    {
        public int TestId { get; set; }
        public int? ExerciseId { get; set; }
        public bool? Status { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}
