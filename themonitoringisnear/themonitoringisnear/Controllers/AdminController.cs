using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using themonitoringisnear.DAL;
using themonitoringisnear.Models;
using themonitoringisnear.CustomClass;

namespace themonitoringisnear.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public ActionResult GroupsIndex()
        {
            var db = new ElectionDbContext();
            var vm = new MultipleModel.CollegeModelVM();

            vm.DbCollege = db.Colleges.Where(c => c.Deleted == false).ToList();

            var tempData = TempData["GroupsIndexTD"] as MultipleModel.CollegeModelVM;

            if(tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
                vm.CreateCollege = tempData.CreateCollege;
            }

            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateCollege(MultipleModel.CollegeModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    if(db.Colleges.Where(c=>c.College == request.CreateCollege.CollegeName).Any())
                    {
                        string message = request.CreateCollege.CollegeName + " is already existing!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        request.Error = true;
                        TempData["GroupsIndexTD"] = request;
                        return RedirectToAction("GroupsIndex", "Admin");
                    }
                    else if(!(db.Colleges.Where(c => c.College == request.CreateCollege.CollegeName).Any()))
                    {
                        var newCollege = db.Colleges.Create();
                        newCollege.College = request.CreateCollege.CollegeName;
                        newCollege.CreatedAt = DateTime.Now;

                        db.Colleges.Add(newCollege);
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully added " + newCollege.College + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["GroupsIndexTD"] = request.Error;
                        TempData["GroupsIndexTD"] = request.Message;

                        return RedirectToAction("GroupsIndex", "Admin");
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["GroupsIndexTD"] = request;
            return RedirectToAction("GroupsIndex", "Admin");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteCollege(MultipleModel.CollegeModelVM request)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var college = db.Colleges.Find(request.DeleteCollege.CollegeId);

                    if (college != null)
                    {
                        college.Deleted = true;
                        college.UpdatedAt = DateTime.Now;
                        db.SaveChanges();

                        request.Error = false;
                        var messageList = new List<string>();
                        string message = "You have successfully deleted " + college.College + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["GroupsIndexTD"] = request;

                        return RedirectToAction("GroupsIndex", "Admin");
                    }
                    else if (college == null)
                    {
                        request.Error = true;
                        var messageList = new List<string>();
                        string message = "The college selected does not exist!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["GroupsIndexTD"] = request;

                        return RedirectToAction("GroupsIndex", "Admin");
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["GroupsIndexTD"] = request;
            return RedirectToAction("GroupsIndex", "Admin");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateCollege(MultipleModel.CollegeModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var colleges = db.Colleges.Find(request.UpdateCollege.CollegeId);

                    if (colleges != null)
                    {
                        if(!(db.Colleges.Where(c=>c.College == request.UpdateCollege.CollegeName).Any()))
                        {
                            colleges.College = request.UpdateCollege.CollegeName;
                            colleges.UpdatedAt = DateTime.Now;

                            db.Entry(colleges).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            request.Error = false;
                            string message = "You have successfully updated " + colleges.College;
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["GroupsIndexTD"] = request.Error;
                            TempData["GroupsIndexTD"] = request.Message;

                            return RedirectToAction("GroupsIndex", "Admin");
                        }
                        else if(db.Colleges.Where(c => c.College == request.UpdateCollege.CollegeName).Any())
                        {
                            request.Error = true;
                            string message = request.UpdateCollege.CollegeName+" is already existing!!";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["GroupsIndexTD"] = request;
                            return RedirectToAction("GroupsIndex", "Admin");
                        }
                        
                    }
                    else if (colleges == null)
                    {
                        request.Error = true;
                        string message = "The selected college does not exist!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["GroupsIndexTD"] = request;
                        return RedirectToAction("GroupsIndex", "Admin");
                    }
                }

            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["GroupsIndexTD"] = request;
            return RedirectToAction("GroupsIndex", "Admin");
        }

        [HttpGet]
        public ActionResult MajorIndex(Guid? id)
        {
            var db = new ElectionDbContext();
            var vm = new MultipleModel.MajorModelVM();
            var tempData = TempData["MajorIndexTD"] as MultipleModel.MajorModelVM;
            vm.DbMajor = db.Majors.Where(m=>m.CollegeId == id).Where(m => m.Deleted == false).ToList();
            vm.CollegeId = id;
            vm.CollegeName = db.Colleges.Find(id).College;
            if(tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
                vm.CreateMajor = tempData.CreateMajor;
            }
             
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateMajor(MultipleModel.MajorModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    if (!(db.Majors.Where(m => m.Major == request.CreateMajor.MajorName).Any()))
                    {
                        var newMajor = db.Majors.Create();
                        newMajor.CollegeId = request.CreateMajor.CollegeId;
                        newMajor.Major = request.CreateMajor.MajorName;
                        newMajor.CreatedAt = DateTime.Now;

                        db.Majors.Add(newMajor);
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully added " + newMajor.Major + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["MajorIndexTD"] = request.Error;
                        TempData["MajorIndexTD"] = request.Message;

                        return RedirectToAction("MajorIndex", new { id = request.CreateMajor.CollegeId });
                    }
                    else if (db.Majors.Where(m => m.Major == request.CreateMajor.MajorName).Any())
                    {
                        request.Error = true;
                        string message = request.CreateMajor.MajorName + " is already existing!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["MajorIndexTD"] = request;

                        return RedirectToAction("MajorIndex", new { id = request.CreateMajor.CollegeId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["MajorIndexTD"] = request;

            return RedirectToAction("MajorIndex", new { id = request.CreateMajor.CollegeId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteMajor(MultipleModel.MajorModelVM request)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var major = db.Majors.Find(request.DeleteMajor.MajorId);

                    if (major != null)
                    {
                        major.Deleted = true;
                        major.UpdatedAt = DateTime.Now;
                        db.SaveChanges();

                        request.Error = false;
                        var messageList = new List<string>();
                        string message = "You have successfully deleted " + major.Major + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["MajorIndexTD"] = request;

                        return RedirectToAction("MajorIndex", new { id = request.CollegeId });
                    }
                    else if (major == null)
                    {
                        request.Error = true;
                        var messageList = new List<string>();
                        string message = "The major selected does not exist!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["MajorIndexTD"] = request;

                        return RedirectToAction("MajorIndex", new { id = request.CollegeId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["MajorIndexTD"] = request;
            return RedirectToAction("MajorIndex", new { id = request.CollegeId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateMajor(MultipleModel.MajorModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var major = db.Majors.Find(request.UpdateMajor.MajorId);
       
                    if (major != null)
                    {
                        if(!(db.Majors.Where(m => m.Major == request.UpdateMajor.MajorName).Any()))
                        {
                            major.Major = request.UpdateMajor.MajorName;
                            major.UpdatedAt = DateTime.Now;

                            db.Entry(major).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            request.Error = false;
                            string message = "You have successfully updated " + major.Major;
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["MajorIndexTD"] = request;
                            return RedirectToAction("MajorIndex", new { id = request.CollegeId });
                        }
                        else if (db.Majors.Where(m => m.Major == request.UpdateMajor.MajorName).Any())
                        {
                            request.Error = true;
                            string message = request.UpdateMajor.MajorName+" is already existing!!";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["MajorIndexTD"] = request;
                            return RedirectToAction("MajorIndex", new { id = request.CollegeId });
                        }
                    }
                    else if (major == null)
                    {
                        request.Error = true;
                        string message = "The selected major does not exist!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["MajorIndexTD"] = request;
                        return RedirectToAction("MajorIndex", new { id = request.CollegeId });
                    }
                }

            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["MajorIndexTD"] = request;
            return RedirectToAction("MajorIndex", new { id = request.CollegeId });
        }

        [HttpGet]
        public ActionResult SectionIndex(Guid? id)
        {
            var db = new ElectionDbContext();
            var vm = new MultipleModel.SectionModelVM();
            var tempData = TempData["SectionIndexTD"] as MultipleModel.SectionModelVM;
            vm.DbSection = db.Sections.Where(s=>s.MajorId == id).Where(m => m.Deleted == false).ToList();
            vm.MajorId = id;
            vm.MajorName = db.Majors.Find(id).Major;
            vm.CollegeId = db.Majors.Find(id).CollegeId;
            if (tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
            }

            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateSection(MultipleModel.SectionModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    if (!(db.Sections.Where(m => m.Section == request.CreateSection.SectionName).Any()))
                    {
                        var newSection = db.Sections.Create();
                        newSection.MajorId = request.CreateSection.MajorId;
                        newSection.Section = request.CreateSection.SectionName;
                        newSection.CreatedAt = DateTime.Now;

                        db.Sections.Add(newSection);
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully added " + newSection.Section + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["SectionIndexTD"] = request.Error;
                        TempData["SectionIndexTD"] = request.Message;

                        return RedirectToAction("SectionIndex", new { id = request.CreateSection.MajorId });
                    }
                    else if (db.Sections.Where(m => m.Section == request.CreateSection.SectionName).Any())
                    {
                        request.Error = true;
                        string message = request.CreateSection.SectionName + " is already existing!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["SectionIndexTD"] = request;

                        return RedirectToAction("SectionIndex", new { id = request.CreateSection.MajorId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["SectionIndexTD"] = request;

            return RedirectToAction("SectionIndex", new { id = request.CreateSection.MajorId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteSection(MultipleModel.SectionModelVM request)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var section = db.Sections.Find(request.DeleteSection.SectionId);

                    if (section != null)
                    {
                        section.Deleted = true;
                        section.UpdatedAt = DateTime.Now;
                        db.SaveChanges();

                        request.Error = false;
                        var messageList = new List<string>();
                        string message = "You have successfully deleted " + section.Section + "";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["SectionIndexTD"] = request;

                        return RedirectToAction("SectionIndex", new { id = request.MajorId });
                    }
                    else if (section == null)
                    {
                        request.Error = true;
                        var messageList = new List<string>();
                        string message = "The section selected does not exist!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["SectionIndexTD"] = request;

                        return RedirectToAction("SectionIndex", new { id = request.MajorId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["SectionIndexTD"] = request;
            return RedirectToAction("SectionIndex", new { id = request.MajorId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateSection(MultipleModel.SectionModelVM request)
        {
            var messageList = new List<string>();
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var section = db.Sections.Find(request.UpdateSection.SectionId);

                    if (section != null)
                    {
                        if (!(db.Sections.Where(m => m.Section == request.UpdateSection.SectionName).Any()))
                        {
                            section.Section = request.UpdateSection.SectionName;
                            section.UpdatedAt = DateTime.Now;

                            db.Entry(section).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            request.Error = false;
                            string message = "You have successfully updated " + section.Section;
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["SectionIndexTD"] = request;
                            return RedirectToAction("SectionIndex", new { id = request.MajorId });
                        }
                        else if (db.Sections.Where(m => m.Section == request.UpdateSection.SectionName).Any())
                        {
                            request.Error = true;
                            string message = request.UpdateSection.SectionName + " is already existing!!";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["SectionIndexTD"] = request;
                            return RedirectToAction("SectionIndex", new { id = request.MajorId });
                        }
                    }
                    else if (section == null)
                    {
                        request.Error = true;
                        string message = "The selected section does not exist!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["SectionIndexTD"] = request;
                        return RedirectToAction("SectionIndex", new { id = request.MajorId });
                    }
                }

            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["SectionIndexTD"] = request;
            return RedirectToAction("SectionIndex", new { id = request.MajorId });
        }

        public ActionResult StudentIndex(Guid? id)
        {
            var db = new ElectionDbContext();
            var vm = new MultipleModel.StudentIndexModelVM();
            var tempData = TempData["StudentIndexTD"] as MultipleModel.StudentIndexModelVM;
            vm.DbStudent = db.Students.Where(s => s.SectionId == id).Where(m => m.Deleted == false).ToList();
            vm.DbUser = db.Users.Where(u => u.Student.SectionId == id).Where(u => u.Deleted == false).ToList();
            vm.SectionId = id;
            vm.SectionName = db.Sections.Find(id).Section;
            vm.MajorId = db.Sections.Find(id).MajorId;
            if (tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
                if(vm.Error == true)
                {
                    vm.CreateStudent = tempData.CreateStudent;
                }
            }

            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateStudent(MultipleModel.StudentIndexModelVM request)
        {
            var messageList = new List<string>();
            if(ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    if(!(db.Students.Where(s => s.StudentNumber == request.CreateStudent.StudentNumber).Any()))
                    {
                        var newStudent = db.Students.Create();
                        newStudent.StudentNumber = request.CreateStudent.StudentNumber;
                        newStudent.LastName = request.CreateStudent.LastName;
                        newStudent.FirstName = request.CreateStudent.FirstName;
                        newStudent.MiddleInitial = request.CreateStudent.MiddleInitial;
                        newStudent.Gender = request.CreateStudent.Gender;
                        newStudent.SectionId = request.CreateStudent.SectionId;
                        newStudent.CreatedAt = DateTime.Now;

                        db.Students.Add(newStudent);
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully added "+ request.CreateStudent.StudentNumber;
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.CreateStudent.SectionId });
                    }
                    else if (db.Students.Where(s => s.StudentNumber == request.CreateStudent.StudentNumber).Any())
                    {
                        request.Error = true;
                        string message = request.CreateStudent.StudentNumber + " is already existing!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.CreateStudent.SectionId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["StudentIndexTD"] = request;
            return RedirectToAction("StudentIndex", new { id = request.CreateStudent.SectionId});
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteStudent(MultipleModel.StudentIndexModelVM request)
        {
            var messageList = new List<string>();

            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var getStudent = db.Students.Find(request.DeleteStudent.StudentId);
                    if (getStudent != null)
                    {
                        getStudent.Deleted = true;
                        getStudent.UpdatedAt = DateTime.Now;

                        db.Entry(getStudent).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully deleted student (" + getStudent.StudentNumber + ")";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.SectionId });
                    }
                    else if (getStudent == null)
                    {
                        request.Error = true;
                        string message = "The selected student does not exist!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.SectionId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["StudentIndexTD"] = request;
            return RedirectToAction("StudentIndex", new { id = request.SectionId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateStudent(MultipleModel.StudentIndexModelVM request)
        {
            var messageList = new List<string>();

            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var getStudent = db.Students.Find(request.UpdateUser.StudentId);
                    if (getStudent != null)
                    {
                        if(!(db.Students.Where(s => s.StudentNumber == request.UpdateUser.StudentNumber).Any()) || (getStudent.StudentNumber == request.UpdateUser.StudentNumber))
                        {
                            getStudent.StudentNumber = request.UpdateUser.StudentNumber;
                            getStudent.LastName = request.UpdateUser.LastName;
                            getStudent.FirstName = request.UpdateUser.FirstName;
                            getStudent.MiddleInitial = request.UpdateUser.MiddleInitial;
                            getStudent.Gender = request.UpdateUser.Gender;
                            getStudent.UpdatedAt = DateTime.Now;

                            db.Entry(getStudent).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            request.Error = false;
                            string message = "You have successfully updated student (" + getStudent.StudentNumber + ")";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["StudentIndexTD"] = request;
                            return RedirectToAction("StudentIndex", new { id = request.SectionId });
                        }
                        else if (db.Students.Where(s => s.StudentNumber == request.UpdateUser.StudentNumber).Any())
                        {
                            request.Error = true;
                            string message = "The student number "+ request.UpdateUser.StudentNumber+" is already existing!!";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["StudentIndexTD"] = request;
                            return RedirectToAction("StudentIndex", new { id = request.SectionId });
                        }
                    }
                    else if (getStudent == null)
                    {
                        request.Error = true;
                        string message = "The selected student does not exist!!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.SectionId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["StudentIndexTD"] = request;
            return RedirectToAction("StudentIndex", new { id = request.SectionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(MultipleModel.StudentIndexModelVM request)
        {
            var messageList = new List<string>();
            if(ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var getStudent = db.Students.SingleOrDefault(s=>s.Id == request.CreateUser.StudentId);
                    if(getStudent != null)
                    {
                        if(!( (db.Users.Where(u => u.StudentId == getStudent.Id).Any()) && (db.Users.Where(u=>u.Email == request.CreateUser.Email).Any()) ))
                        {
                            string pin = SimpleCrypto.RandomPassword.Generate(6, SimpleCrypto.PasswordGroup.Lowercase, SimpleCrypto.PasswordGroup.Lowercase, SimpleCrypto.PasswordGroup.Numeric);
                            var crypto = new SimpleCrypto.PBKDF2();
                            var encrypPin = crypto.Compute(pin);

                            var newUser = db.Users.Create();
                            newUser.StudentId = request.CreateUser.StudentId;
                            newUser.Email = request.CreateUser.Email;
                            newUser.Role = request.CreateUser.Role;
                            newUser.Pincode = encrypPin;
                            newUser.PincodeSalt = crypto.Salt;
                            newUser.CreatedAt = DateTime.Now;

                            db.Users.Add(newUser);
                            db.SaveChanges();

                            SMTP smtp = new SMTP();
                            smtp.SendEmal(newUser.Email, pin);

                            request.Error = false;
                            string message = "You have successfully created a user " + newUser.Email;
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["StudentIndexTD"] = request;
                            return RedirectToAction("StudentIndex", new { id = request.SectionId });
                        }
                        else if ((db.Users.Where(u => u.StudentId == getStudent.Id).Any()) && (db.Users.Where(u => u.Email == request.CreateUser.Email).Any()))
                        {
                            request.Error = true;
                            string message = "Failed to create a user. Email address or Student Id is already existing";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["StudentIndexTD"] = request;
                            return RedirectToAction("StudentIndex", new { id = request.SectionId });
                        }
                    }
                    else if (getStudent == null)
                    {
                        request.Error = true;
                        string message = "Failed to create a user. The selected student does not exist";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["StudentIndexTD"] = request;
                        return RedirectToAction("StudentIndex", new { id = request.SectionId });
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["StudentIndexTD"] = request;
            return RedirectToAction("StudentIndex", new { id = request.SectionId });
        }

        [HttpGet]
        public ActionResult ElectionsIndex()
        {
            var db = new ElectionDbContext();
            var vm = new MultipleModel.ElectionIndexVM();

            vm.DbElection = db.Elections.Where(c => c.Deleted == false).ToList();
            vm.DbCollege = db.Colleges.Where(c => c.Deleted == false).ToList();
            var tempData = TempData["ElectionIndexTD"] as MultipleModel.ElectionIndexVM;

            if (tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
                if(vm.Error == true)
                {
                    tempData.CreateElection.StartDate.ToShortDateString();
                    tempData.CreateElection.StartTime.ToShortTimeString();
                    tempData.CreateElection.EndDate.ToShortDateString();
                    tempData.CreateElection.EndTime.ToShortTimeString();

                    vm.CreateElection = tempData.CreateElection;
                }
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateElection(MultipleModel.ElectionIndexVM request)
        {
            var messageList = new List<string>();
            if(ModelState.IsValid)
            {
                using( var db = new ElectionDbContext())
                {
                    if(!(db.Elections.Where(e => e.ElectionName == request.CreateElection.ElectionName).Any()))
                    {
                        var newElection = db.Elections.Create();
                        var startDate = request.CreateElection.StartDate;
                        var startTime = request.CreateElection.StartTime;
                        var endDate = request.CreateElection.EndDate;
                        var endTime = request.CreateElection.EndTime;

                        if (request.CreateElection.GroupId != null)
                        {
                            newElection.GroupId = request.CreateElection.GroupId;
                        }
                        newElection.ElectionName = request.CreateElection.ElectionName;
                        newElection.Id = request.CreateElection.GroupId;
                        newElection.StartDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                        newElection.EndDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                        newElection.CreatedAt = DateTime.Now;

                        db.Elections.Add(newElection);
                        db.SaveChanges();

                        request.Error = false;
                        string message = "You have successfully created " + newElection.ElectionName;
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["ElectionIndexTD"] = request;
                        return RedirectToAction("ElectionsIndex", "Admin");
                    }
                    else if (db.Elections.Where(e => e.ElectionName == request.CreateElection.ElectionName).Any())
                    {
                        request.Error = true;
                        string message = request.CreateElection.ElectionName + " is already existing!";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["ElectionIndexTD"] = request;
                        return RedirectToAction("ElectionsIndex", "Admin");
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["ElectionIndexTD"] = request;
            return RedirectToAction("ElectionsIndex", "Admin");
        }
    }
}