using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using themonitoringisnear.Models;
using themonitoringisnear.DAL;

namespace themonitoringisnear.Models
{
    public class MultipleModel
    {
        public class LoginPageVM
        {
            public ActivationModel Activation { get; set; }
            public AuthModel Auth { get; set; }

            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class ActivateVM
        {
            public ElectionDbContext.UserModel DbUser { get; set; }
            public AccountActivationModel AccountActivation { get; set; }

            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }
        public class CollegeModelVM
        {
            public ICollection<ElectionDbContext.CollegeModel> DbCollege { get; set; }
            public CreateCollegeModel CreateCollege { get; set; }
            public DeleteCollegeModel DeleteCollege { get; set; }
            public UpdateCollegeModel UpdateCollege { get; set; }

            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class MajorModelVM
        {
            public ICollection<ElectionDbContext.MajorModel> DbMajor { get; set; }
            public CreateMajorModel CreateMajor { get; set; }
            public DeleteMajorModel DeleteMajor { get; set; }
            public UpdateMajorModel UpdateMajor { get; set; }

            public Guid? CollegeId { get; set; }
            public string CollegeName { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class SectionModelVM
        {
            public ICollection<ElectionDbContext.SectionModel> DbSection { get; set; }
            public CreateSectionModel CreateSection { get; set; }
            public DeleteSectionModel DeleteSection { get; set; }
            public UpdateSectionModel UpdateSection { get; set; }

            public Guid? CollegeId { get; set; }
            public Guid? MajorId { get; set; }
            public string MajorName { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class StudentIndexModelVM
        {
            public ICollection<ElectionDbContext.StudentModel> DbStudent { get; set; }
            public ICollection<ElectionDbContext.UserModel> DbUser { get; set; }
            public CreateUser CreateUser { get; set; }
            public CreateStudentModel CreateStudent { get; set; }
            public DeleteStudentModel DeleteStudent { get; set; }
            public UpdateStudentModel UpdateUser { get; set; }

            public Guid? MajorId { get; set; }
            public Guid? SectionId { get; set; }
            public string SectionName { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class ElectionIndexVM
        {
            public ICollection<ElectionDbContext.ElectionModel> DbElection { get; set; }
            public ICollection<ElectionDbContext.CollegeModel> DbCollege { get; set; }
            public CreateElectionModel CreateElection { get; set; }

            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }
    }
}