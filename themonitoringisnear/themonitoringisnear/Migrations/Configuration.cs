namespace themonitoringisnear.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using themonitoringisnear.DAL;
    using SimpleCrypto;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<themonitoringisnear.DAL.ElectionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(themonitoringisnear.DAL.ElectionDbContext context)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute("rodnerraymundo");
            string pin = RandomPassword.Generate(6, PasswordGroup.Lowercase, PasswordGroup.Lowercase, PasswordGroup.Numeric);
            var cryptoPin = new SimpleCrypto.PBKDF2();
            var encrypPin = cryptoPin.Compute(pin);

            var colleges = new List<themonitoringisnear.DAL.ElectionDbContext.CollegeModel>
            {
                new DAL.ElectionDbContext.CollegeModel
                {
                    College ="Institute of Information and Computing Sciences", CreatedAt = DateTime.Now,
                    Majors = new List<DAL.ElectionDbContext.MajorModel>
                    {
                        context.Majors.SingleOrDefault(m=>m.Major == "Information Technology")
                    } 
                }
            };
            colleges.ForEach(c => context.Colleges.AddOrUpdate(c));

            var majors = new List<themonitoringisnear.DAL.ElectionDbContext.MajorModel>
            {
                new DAL.ElectionDbContext.MajorModel
                {
                    Major = "Information Technology", CreatedAt = DateTime.Now,
                    Sections = new List<DAL.ElectionDbContext.SectionModel>
                    {
                        context.Sections.SingleOrDefault(s=>s.Section == "4ITA")
                    }
                }
            };
            majors.ForEach(m => context.Majors.AddOrUpdate(m));

            var sections = new List<themonitoringisnear.DAL.ElectionDbContext.SectionModel>
            {
                new DAL.ElectionDbContext.SectionModel
                {
                    Section = "4ITA", CreatedAt = DateTime.Now
                }
            };
            sections.ForEach(s => context.Sections.AddOrUpdate(s));

            var accounts = new List<themonitoringisnear.DAL.ElectionDbContext.UserModel>
            {

                new DAL.ElectionDbContext.UserModel
                {
                    Email = "rodnerraymundo@gmail.com",
                    Password = encrypPass, PasswordSalt = crypto.Salt, Pincode = encrypPin, PincodeSalt = cryptoPin.Salt,
                    Role = "administrator", SecretQuestion = "Who are you?", SecretAnswer = "rodnerraymundo",
                    CreatedAt = DateTime.Now,
                    Status = true,
                },
            };
            accounts.ForEach(a => context.Users.AddOrUpdate(a));

            var information = new List<themonitoringisnear.DAL.ElectionDbContext.StudentModel>
            {
                new DAL.ElectionDbContext.StudentModel
                {
                    StudentUID = "2d5d0f24", StudentNumber="2012042055", FirstName = "Rodner", MiddleInitial = "Y", LastName = "Raymundo", Status = true,
                    Gender = "male", CreatedAt = DateTime.Now, Section = context.Sections.SingleOrDefault(s=>s.Section == "4ITA")
                }
            };
            information.ForEach(i => context.Students.AddOrUpdate(i));
            

            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
