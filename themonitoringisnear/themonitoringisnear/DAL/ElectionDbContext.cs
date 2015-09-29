using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel;

namespace themonitoringisnear.DAL
{
    public class ElectionDbContext : DbContext
    {
        public ElectionDbContext() : base ("ElectionDbContext")
        {
            Database.SetInitializer<ElectionDbContext>(new CreateDatabaseIfNotExists<ElectionDbContext>());
            Database.SetInitializer<ElectionDbContext>(new DropCreateDatabaseIfModelChanges<ElectionDbContext>());
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<SectionModel> Sections { get; set; }
        public DbSet<MajorModel> Majors { get; set; }
        public DbSet<CollegeModel> Colleges { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        public DbSet<ElectionModel> Elections { get; set; }
        public DbSet<RoomsModel> Rooms { get; set; }
        public DbSet<TerminalsModel> Terminals { get; set; }
        public DbSet<AttendanceLogsModel> AttendanceLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        [Table("Users")]
        public class UserModel
        {
            [Column(Order = 0), Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Image")]
            public Guid? ImageId { get; set; }
            [ForeignKey("Student")]
            public Guid StudentId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordSalt { get; set; }
            public string Pincode { get; set; }
            public string PincodeSalt { get; set; }
            public string Role { get; set; }
            public string SecretQuestion { get; set; }
            public string SecretAnswer { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public bool Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual StudentModel Student { get; set; }
            public virtual ImageModel Image { get; set; }
        }

        [Table("Students")]
        public class StudentModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Section")]
            public Guid SectionId { get; set; }
            public string StudentUID { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string StudentNumber { get; set; }
            public string FirstName { get; set; }
            [MaxLength(1)]
            [MinLength(1)]
            public string MiddleInitial { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            [DefaultValue("false")]
            public bool Status { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual SectionModel Section { get; set; }
        }
        [Table("Images")]
        public class ImageModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid ImageId { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

        }

        [Table("Sections")]
        public class SectionModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Major")]
            public Guid MajorId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Section { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual MajorModel Major { get; set; }

        }

        [Table("Majors")]
        public class MajorModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("College")]
            public Guid CollegeId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Major { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public ICollection<SectionModel> Sections { get; set; }
            public virtual CollegeModel College { get; set; }
        }

        [Table("Colleges")]
        public class CollegeModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string College { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public ICollection<MajorModel> Majors { get; set; }
        }

        [Table("Elections")]
        public class ElectionModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            public string ElectionName { get; set; }
            public Guid? GroupId { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            [DefaultValue(false)]
            public bool Enable { get; set; }
            [DefaultValue(false)]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual ICollection<AttendanceLogsModel> AttendanceLogs{ get; set; }
            public virtual ICollection<RoomsModel> Rooms{ get; set; }
        }

        [Table("AttendanceLogs")]
        public class AttendanceLogsModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Election")]
            public Guid ElectionId { get; set; }
            [ForeignKey("Student")]
            public Guid StudentId { get; set; }
            public string MessageLog { get; set; }
            public string TakenAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual ElectionModel Election { get; set; }
            public virtual StudentModel Student { get; set; }
        }

        [Table("Rooms")]
        public class RoomsModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Election")]
            public Guid ElectionId { get; set; }
            [ForeignKey("College")]
            public Guid CollegeId { get; set; }
            public string RoomName { get; set; }
            public int TerminalNumbers { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual CollegeModel College { get; set; }
            public virtual ElectionModel Election { get; set; }
            public virtual ICollection<TerminalsModel> Terminals{ get; set; }
        }

        [Table("Terminals")]
        public class TerminalsModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Room")]
            public Guid RoomId { get; set; }
            public Guid StudentId { get; set; }
            public string TerminalNumber { get; set; }
            [DefaultValue(false)]
            public bool Status { get; set; }
            [DefaultValue(true)]
            public bool Enable { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual RoomsModel Room { get; set; }
        }
    }
}