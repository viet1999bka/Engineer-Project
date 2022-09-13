using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DATN.Models
{
    public partial class EndProjectContext : DbContext
    {
        public EndProjectContext()
        {
        }

        public EndProjectContext(DbContextOptions<EndProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Challenge> Challenge { get; set; }
        public virtual DbSet<CodeFriend> CodeFriend { get; set; }
        public virtual DbSet<Difficult> Difficult { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Exercise> Exercise { get; set; }
        public virtual DbSet<ExerciseAttemp> ExerciseAttemp { get; set; }
        public virtual DbSet<Experience> Experience { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LanguageSupport> LanguageSupport { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<TestCase> TestCase { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=PVIET\\SQLEXPRESS; Database=EndProject; Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.Property(e => e.ChallengeName).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Thumb).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<CodeFriend>(entity =>
            {
                entity.HasKey(e => e.Cfid);

                entity.Property(e => e.Cfid).HasColumnName("CFID");

                entity.Property(e => e.DateJoin).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.CodeFriendFriend)
                    .HasForeignKey(d => d.FriendId)
                    .HasConstraintName("FK_CodeFriend_Account1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CodeFriendUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CodeFriend_Account");
            });

            modelBuilder.Entity<Difficult>(entity =>
            {
                entity.Property(e => e.DifficultId).HasColumnName("DifficultID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DifficultName).HasMaxLength(50);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.Property(e => e.EducationId).HasColumnName("EducationID");

                entity.Property(e => e.Degree).HasMaxLength(200);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.Property(e => e.University).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Education)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Education_Account");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.ExcerciseId);

                entity.Property(e => e.ExcerciseId).HasColumnName("ExcerciseID");

                entity.Property(e => e.ChallengeId).HasColumnName("ChallengeID");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Example).HasMaxLength(500);

                entity.Property(e => e.ExcerciseName).HasMaxLength(50);

                entity.Property(e => e.Input).HasMaxLength(100);

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.Output).HasMaxLength(100);

                entity.Property(e => e.TimeLimit)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.Challenge)
                    .WithMany(p => p.Exercise)
                    .HasForeignKey(d => d.ChallengeId)
                    .HasConstraintName("FK_Exercise_Challenge");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Exercise)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_Exercise_Difficult");
            });

            modelBuilder.Entity<ExerciseAttemp>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodeSubmit).HasColumnType("text");

                entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseAttemp)
                    .HasForeignKey(d => d.ExerciseId)
                    .HasConstraintName("FK_ExerciseAttemp_Exercise");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ExerciseAttemp)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_ExerciseAttemp_Language");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExerciseAttemp)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExerciseAttemp_Account");
            });

            modelBuilder.Entity<Experience>(entity =>
            {
                entity.Property(e => e.ExperienceId).HasColumnName("ExperienceID");

                entity.Property(e => e.Company).HasMaxLength(150);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.JobLocation).HasColumnType("text");

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Experience)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Experience_Account");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.LanguageExten)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.LanguageMode)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LanguageName)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.LạnguageDisplay)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<LanguageSupport>(entity =>
            {
                entity.HasKey(e => e.LanguageSupId);

                entity.Property(e => e.LanguageSupId).HasColumnName("LanguageSupID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

                entity.Property(e => e.FileFunction).HasMaxLength(200);

                entity.Property(e => e.FileMain).HasMaxLength(200);

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.LanguageSupport)
                    .HasForeignKey(d => d.ExerciseId)
                    .HasConstraintName("FK_LanguageSupport_Exercise");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageSupport)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_LanguageSupport_Language");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.RoleName).HasMaxLength(150);
            });

            modelBuilder.Entity<TestCase>(entity =>
            {
                entity.HasKey(e => e.TestId);

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

                entity.Property(e => e.Input).HasColumnType("text");

                entity.Property(e => e.Output).HasColumnType("text");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.TestCase)
                    .HasForeignKey(d => d.ExerciseId)
                    .HasConstraintName("FK_TestCase_Exercise");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
