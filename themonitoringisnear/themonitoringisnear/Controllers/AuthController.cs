using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using themonitoringisnear.DAL;
using themonitoringisnear.CustomClass;
using themonitoringisnear.Models;
using System.Security.Claims;

namespace themonitoringisnear.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            if (authManager.User.IsInRole("administrator"))
            {
                return RedirectToAction("GroupsIndex", "Admin");
            }
            else if(authManager.User.IsInRole("localadministrator"))
            {
                return RedirectToAction("Index", "User");
            }
            else if (authManager.User.IsInRole("student"))
            {
                return RedirectToAction("Index", "Auth");
            }

            var loginVM = new MultipleModel.LoginPageVM();
            var loginTD = TempData["LoginTempData"] as MultipleModel.LoginPageVM;
            
            if(loginTD != null)
            {
                loginVM.Error = loginTD.Error;
                loginVM.Message = loginTD.Message;
                if(loginVM.Error == false)
                {
                    loginVM.Auth = loginTD.Auth;
                    loginVM.Activation = loginTD.Activation;
                }
            }

            return View(loginVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(MultipleModel.LoginPageVM request)
        {
            if(ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var emailCheck = db.Users.SingleOrDefault(u => u.Email == request.Auth.Email);
                    var getPasswordSalt = emailCheck.PasswordSalt;

                    if ((emailCheck != null) && (getPasswordSalt != null) && (emailCheck.Deleted == false) && (emailCheck.Status == true))
                    {
                        var materializePasswordSalt = getPasswordSalt.ToList();
                        var encryptedPassword = crypto.Compute(request.Auth.Password, getPasswordSalt);

                        if (request.Auth.Email != null && emailCheck.Password == encryptedPassword)
                        {
                            var fullname = emailCheck.Student.FirstName + " " + emailCheck.Student.MiddleInitial + " " + emailCheck.Student.LastName;
                            var email = emailCheck.Email;
                            var role = emailCheck.Role;

                            var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, fullname),
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, role)
                        }, "ApplicationCookie");
                            
                            var ctx = Request.GetOwinContext();
                            var authManager = ctx.Authentication;
                            authManager.SignIn(identity);

                            if (emailCheck.Role == "administrator")
                            {
                                return RedirectToAction("GroupsIndex", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Auth");
                            }
                        }
                        else
                        {
                            request.Error = true;
                            ModelState.AddModelError("", "Invalid email or password");
                        }
                    }
                    else if ((emailCheck != null) && (emailCheck.Status == false) && (emailCheck.Deleted == false))
                    {
                        request.Error = true;
                        ModelState.AddModelError("", "Please activate the account");
                    }
                    else if (emailCheck == null || ((emailCheck.Deleted == true) && (emailCheck.Status == false)))
                    {
                        request.Error = true;
                        ModelState.AddModelError("", "Account does not exist");
                    }
                }

            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["LoginTempData"] = request;

            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateAccount(MultipleModel.LoginPageVM request)
        {
            var messageList = new List<string>();
            
            if (ModelState.IsValid)
            {
                using (var db = new ElectionDbContext())
                {
                    var getUser = db.Users.Where(u=>u.Deleted == false).SingleOrDefault(u => u.Email == request.Activation.Email);
                    if(getUser != null)
                    {
                        if(getUser.Status == false)
                        {
                            var crypto = new SimpleCrypto.PBKDF2();
                            var encryptedPinCode = crypto.Compute(request.Activation.Pincode, getUser.PincodeSalt);
                            if (getUser.Pincode == encryptedPinCode)
                            {
                                var ctx = Request.GetOwinContext();
                                var authManager = ctx.Authentication;

                                var identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, getUser.Email),
                                new Claim(ClaimTypes.Role, "activation")
                                }, "ApplicationCookie");

                                authManager.SignIn(identity);
                                return RedirectToAction("ActivateAccount", new { id = getUser.Id });
                            }
                            else if (getUser.Pincode != encryptedPinCode)
                            {
                                request.Error = true;
                                string message = "Invalid pincode";
                                messageList.Add(message);
                                request.Message = messageList;
                                TempData["LoginTempData"] = request;

                                return RedirectToAction("Login", "Auth");
                            }
                        }
                        else if (getUser.Status == true)
                        {
                            request.Error = true;
                            string message = getUser.Email +" is already activated";
                            messageList.Add(message);
                            request.Message = messageList;
                            TempData["LoginTempData"] = request;

                            return RedirectToAction("Login", "Auth");
                        }
                    }
                    else if (getUser == null)
                    {
                        request.Error = true;
                        string message = "Invalid email address";
                        messageList.Add(message);
                        request.Message = messageList;
                        TempData["LoginTempData"] = request;

                        return RedirectToAction("Login", "Auth");
                    }
                }
            }
            request.Error = true;
            request.Message = CustomClass.CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["LoginTempData"] = request;
                
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [Authorize(Roles ="activation")]
        public ActionResult ActivateAccount(Guid id)
        {
            var vm = new MultipleModel.ActivateVM();
            var tempData = TempData["ActivateAccountTD"] as MultipleModel.ActivateVM;
            var db = new ElectionDbContext();
            vm.DbUser = db.Users.Find(id);
            if(tempData != null)
            {
                vm.Error = tempData.Error;
                vm.Message = tempData.Message;
                if(vm.Error == true)
                {
                    vm.AccountActivation = tempData.AccountActivation;
                }
            }
            return View(vm);
        }
    }
}