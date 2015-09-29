namespace themonitoringisnear.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElectionDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendanceLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ElectionId = c.Guid(nullable: false),
                        StudentId = c.Guid(nullable: false),
                        MessageLog = c.String(),
                        TakenAt = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Elections", t => t.ElectionId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ElectionId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Elections",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ElectionName = c.String(),
                        GroupId = c.Guid(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ElectionId = c.Guid(nullable: false),
                        CollegeId = c.Guid(nullable: false),
                        RoomName = c.String(),
                        TerminalNumbers = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colleges", t => t.CollegeId, cascadeDelete: true)
                .ForeignKey("dbo.Elections", t => t.ElectionId, cascadeDelete: true)
                .Index(t => t.ElectionId)
                .Index(t => t.CollegeId);
            
            CreateTable(
                "dbo.Colleges",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        College = c.String(maxLength: 450),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.College, unique: true);
            
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CollegeId = c.Guid(nullable: false),
                        Major = c.String(maxLength: 450),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colleges", t => t.CollegeId, cascadeDelete: true)
                .Index(t => t.CollegeId)
                .Index(t => t.Major, unique: true);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        MajorId = c.Guid(nullable: false),
                        Section = c.String(maxLength: 450),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Majors", t => t.MajorId, cascadeDelete: true)
                .Index(t => t.MajorId)
                .Index(t => t.Section, unique: true);
            
            CreateTable(
                "dbo.Terminals",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RoomId = c.Guid(nullable: false),
                        StudentId = c.Guid(nullable: false),
                        TerminalNumber = c.String(),
                        Status = c.Boolean(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SectionId = c.Guid(nullable: false),
                        StudentUID = c.String(),
                        StudentNumber = c.String(maxLength: 450),
                        FirstName = c.String(),
                        MiddleInitial = c.String(maxLength: 1),
                        LastName = c.String(),
                        Gender = c.String(),
                        Status = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId)
                .Index(t => t.StudentNumber, unique: true);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                        Name = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ImageId = c.Guid(),
                        StudentId = c.Guid(nullable: false),
                        Email = c.String(maxLength: 450),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        Pincode = c.String(),
                        PincodeSalt = c.String(),
                        Role = c.String(),
                        SecretQuestion = c.String(),
                        SecretAnswer = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ImageId)
                .Index(t => t.StudentId)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Users", "ImageId", "dbo.Images");
            DropForeignKey("dbo.AttendanceLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.AttendanceLogs", "ElectionId", "dbo.Elections");
            DropForeignKey("dbo.Terminals", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "ElectionId", "dbo.Elections");
            DropForeignKey("dbo.Rooms", "CollegeId", "dbo.Colleges");
            DropForeignKey("dbo.Sections", "MajorId", "dbo.Majors");
            DropForeignKey("dbo.Majors", "CollegeId", "dbo.Colleges");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "StudentId" });
            DropIndex("dbo.Users", new[] { "ImageId" });
            DropIndex("dbo.Students", new[] { "StudentNumber" });
            DropIndex("dbo.Students", new[] { "SectionId" });
            DropIndex("dbo.Terminals", new[] { "RoomId" });
            DropIndex("dbo.Sections", new[] { "Section" });
            DropIndex("dbo.Sections", new[] { "MajorId" });
            DropIndex("dbo.Majors", new[] { "Major" });
            DropIndex("dbo.Majors", new[] { "CollegeId" });
            DropIndex("dbo.Colleges", new[] { "College" });
            DropIndex("dbo.Rooms", new[] { "CollegeId" });
            DropIndex("dbo.Rooms", new[] { "ElectionId" });
            DropIndex("dbo.AttendanceLogs", new[] { "StudentId" });
            DropIndex("dbo.AttendanceLogs", new[] { "ElectionId" });
            DropTable("dbo.Users");
            DropTable("dbo.Images");
            DropTable("dbo.Students");
            DropTable("dbo.Terminals");
            DropTable("dbo.Sections");
            DropTable("dbo.Majors");
            DropTable("dbo.Colleges");
            DropTable("dbo.Rooms");
            DropTable("dbo.Elections");
            DropTable("dbo.AttendanceLogs");
        }
    }
}
