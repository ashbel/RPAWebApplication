using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Interfaces;
using RpaUi.Services;

namespace RpaUi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMyEmailSender _emailSender;
        private readonly ApplicationDbContext _context;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IMyEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel

        {
            [Required]
            
            [Display(Name = "Name")]
            public string firstName { get; set; }

            [Required]
           
            [Display(Name = "Surname")]
            public string lastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            public DateTime dateOfBirth { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string gender { get; set; }

            [Required]
            [Display(Name = "PCZ Practice Number")]
            public string RpaNumber { get; set; }

            [Required]
            [Display(Name = "Pharmacy")]
            public int PharmacyId { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Work Address")]
            public string workAddress { get; set; }

            //[Required]
            [Display(Name = "Qualifications (Tick all that apply)")]
            public string qualifications { get; set; }

            [Required]
            [Display(Name = "Years in practice post pre-registration qualification ")]
            public int yearsInPractice { get; set; }

            [Required]
            [Display(Name = "When did you join RPA")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
            public DateTime dateOfJoiningRPA { get; set; }


            [Required]
            [Display(Name = "Are you in good standing with Pharmacists Council of Zimbabwe")]
            public bool goodStandingPCZ { get; set; }

            [StringLength(500)]
            [Display(Name = "If no, kindly explain")]
            public string goodStandingReasonPCZ { get; set; }

            [Required]
            [Display(Name = "Are you in good standing with the Medicines Control Authority of Zimbabwe")]
            public bool goodStandingMCAZ { get; set; }


            [StringLength(500)]
            [Display(Name = "If no, kindly explain")]
            public string goodStandingReasonMCAZ { get; set; }

            [StringLength(500)]
            [Display(Name = "Other Qualifications")]
            public string otherQualification { get; set; }

            public bool profileComplete { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            ViewData["Qualification"] = _context.tblQualifications;
            ViewData["PharmacyId"] = new SelectList(_context.tblPharmacies.OrderBy(c=>c.pharmacyName), "id", "pharmacyName");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, DateOfBirth = Input.dateOfBirth, FirstName = Input.firstName, Gender = Input.gender, 
                    PhoneNumber = Input.PhoneNumber, LastName = Input.lastName, RpaNumber = Input.RpaNumber, FullName = Input.firstName + " " + Input.lastName };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //Add Users To Role
                    var role = await _userManager.AddToRoleAsync(user, "Client");
                    //create Client
                    tblPharmacists tblPharmacist = new tblPharmacists();
                    tblPharmacist.ApplicationUserId = user.Id;
                    tblPharmacist.tblPharmacyid = Input.PharmacyId;
                    tblPharmacist.profileComplete = false;
                    tblPharmacist.Created = DateTime.Now;
                    tblPharmacist.workAddress = Input.workAddress;
                    tblPharmacist.goodStandingMCAZ = Input.goodStandingMCAZ;
                    tblPharmacist.goodStandingPCZ = Input.goodStandingPCZ;
                    tblPharmacist.goodStandingReasonMCAZ = Input.goodStandingReasonMCAZ;
                    tblPharmacist.goodStandingReasonPCZ = Input.goodStandingReasonPCZ;
                    tblPharmacist.dateOfJoiningRPA = Input.dateOfJoiningRPA;
                    tblPharmacist.yearsInPractice = Input.yearsInPractice.ToString();
                    tblPharmacist.status = false;
                    //save 
                    _context.Add(tblPharmacist);
                    await _context.SaveChangesAsync();

                    
                    string[] arr = Request.Form["Input.qualifications"].ToArray();
                    foreach(var i in arr)
                    {
                        if (i != "false")
                        {
                            tblQualifications_Pharmacist pharmacist_quals = new tblQualifications_Pharmacist();
                            pharmacist_quals.Created = DateTime.Now;
                            pharmacist_quals.QualificationId = Convert.ToInt32(i);
                            pharmacist_quals.PharmacistId = tblPharmacist.Id;

                            _context.Add(pharmacist_quals);
                            await _context.SaveChangesAsync();
                        }
                    }

                    BackgroundJob.Enqueue(() => SendEmailJobAsync(user));

                    await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Input.qualifications = Request.Form["Input.qualifications"].ToString();
            ViewData["Qualification"] = _context.tblQualifications;
            ViewData["PharmacyId"] = new SelectList(_context.tblPharmacies.OrderBy(c => c.pharmacyName), "id", "pharmacyName");
            return Page();
        }

        public async Task SendEmailJobAsync(ApplicationUser user)
        {
            await _emailSender.SendEmailAsync(user.Email, "RPA Registration",
                        $"<p> Dear <b>" + user.FullName + $"</b> </p>  Thank you for registering. <p> We are reviewing your application and we will get back to you via email soon" +
                        $".</p>");
        }
    }
}
