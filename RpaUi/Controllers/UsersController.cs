using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RpaData.Models;
using RpaData.Models.ViewModels;

namespace RpaUi.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IEmailSender _emailSender;

        public UsersController(
                                UserManager<ApplicationUser> usrMgr,
                                RoleManager<IdentityRole> roleMgr, 
                                IPasswordHasher<ApplicationUser> passwordHash,
                                 IEmailSender emailSender
                            )
        {
            userManager = usrMgr;
            roleManager = roleMgr;
            passwordHasher = passwordHash;
            _emailSender = emailSender;
        }


        public async Task<IActionResult> IndexAsync()
        {
            var users = userManager.Users;
            var userslist = new List<UserViewModel>();
            foreach(var user in users.ToList())
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.FullName = user.FullName;
                userViewModel.UserName = user.UserName;
                userViewModel.Email = user.Email;
                userViewModel.Id = user.Id;
                userViewModel.Roles = string.Join(",", userManager.GetRolesAsync(user).Result);

                userslist.Add(userViewModel);
            };
            return View(userslist);
        }

        public ViewResult Create() {

            ViewData["Roles"] = new SelectList(roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser 
                { 
                    UserName = user.Email, 
                    Email = user.Email, 
                    DateOfBirth = user.dateOfBirth, 
                    FirstName = user.firstName, 
                    Gender = user.gender, 
                    PhoneNumber = user.PhoneNumber, 
                    LastName = user.lastName,
                    FullName = user.firstName + " "+user.lastName,
                    RpaNumber = user.RpaNumber,
                    EmailConfirmed = true,
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    //Add Users To Role
                    var role = await userManager.AddToRoleAsync(appUser, user.Roles);
                    //var code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
                   // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = appUser.Id, code = code, returnUrl = "" },
                    //    protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(appUser.Email, "Account Created",
                        $"<p> Dear <b>" + appUser.FullName + $"</b> </p>  An account has been created for you.");

                    return RedirectToAction("Index");

                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            ViewData["Roles"] = new SelectList(roleManager.Roles, "Name", "Name", user.Roles);
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUser user)
        {
            //ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                //if (!string.IsNullOrEmpty(email))
                //    user.Email = email;
                //else
                //    ModelState.AddModelError("", "Email cannot be empty");

                //if (!string.IsNullOrEmpty(password))
                //    user.PasswordHash = passwordHasher.HashPassword(user, password);
                //else
                //    ModelState.AddModelError("", "Password cannot be empty");

                //if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                //{
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
                //}
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}