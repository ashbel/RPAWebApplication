using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Models;

namespace RpaUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private string[] monthStrings =
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Home()
        {

            ViewBag.Client = _context.tblPharmacists.Count();
            ViewBag.ClientsMonth = _context.tblPharmacists.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.Vehicles = _context.tblEvents.Count();
            ViewBag.VehiclesMonth = _context.tblEvents.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.WashesCount = _context.tblPayments.Count();
            ViewBag.WashesCountMonth = _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.Washes = _context.tblPayments.Sum(c => c.AmountPaid).ToString("$#,##0.00");
            ViewBag.WashesMonth = _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Sum(c => c.AmountPaid).ToString("$#,##0.00");

            return View();
        }

        public JsonResult MultiBarMembersGender()
        {
            decimal number;

            var data = _context.tblPharmacists.Include(t => t.ApplicationUser)
                .GroupBy(c => new { c.ApplicationUser.Gender, c.status })
                                                     .Select(x => new
                                                     {
                                                         x.Key.Gender,
                                                         x.Key.status,
                                                         Count = x.Count()
                                                     }).ToList();
            decimal?[] target = new decimal?[data.Count()];
            int i = 0, x = 0;
            foreach (var u in data)
            {
                try
                {
                    target[i] = u.Count;
                }
                catch
                {
                    target[i] = 0;
                }
                i++;
            }

            ChartModel _chart = new ChartModel();
            _chart.labels = data.Select(c => c.Gender).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Number",
                data = target,
                backgroundColor = GetRandomColour(data.Count(),"backGround"),
                borderColor = GetRandomColour(data.Count(),"borderColor"),
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
        }

        public JsonResult MultiBarMembersAge()
        {
            var ceilings = new[] { 30, 40, 50,60,200 };
            var ceilingStr = new[] {"<30 Yrs", "30-40 Yrs", "40-50 Yrs", "50-60 Yrs", "60+"};
            var thisYear = DateTime.Now.Year;
            decimal?[] target = new decimal?[5];
            try
            {
                var data = _context.tblPharmacists.Include(t => t.ApplicationUser).ToList()
                    .GroupBy(c => ceilings.First(ceiling => ceiling >= getAge(c.ApplicationUser.DateOfBirth.Year)))
                    .Select(x => new
                    {
                        x.Key,
                        Count = x.Count()
                    });

                foreach (var u in data)
                {
                    target[Array.IndexOf(ceilings, u.Key)] = u.Count;
                }
            }
            catch (Exception error)
            {

            }

            ChartModel _chart = new ChartModel();
            _chart.labels = ceilingStr;
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Number",
                data = target,
                backgroundColor = GetRandomColour(5,"backGround"),
                borderColor = GetRandomColour(5,"borderColor"),
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
        }

        public JsonResult MultiBarMembership()
        {
            try
            {
                var data = _context.tblMembershipClients.Include(t => t.tblMembership).ToList()
                    .GroupBy(c => c.tblMembership.name)
                    .Select(x => new
                    {
                        x.Key,
                        Count = x.Count()
                    }).ToList();
                decimal?[] target = new decimal?[data.Count()];
                int i = 0;
                foreach (var u in data)
                {
                    target[i] = u.Count;
                    i++;
                }

                ChartModel _chart = new ChartModel();
            _chart.labels = data.Select(c=>c.Key).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Number",
                data = target,
                backgroundColor = GetRandomColour(5,"backGround"),
                borderColor = GetRandomColour(5,"borderColor"),
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
            }
            catch (Exception error)
            {
                return null;
            }
        }

        public int getAge(int Year) => DateTime.Now.Year - Year;
        
        public JsonResult MeetingsBarChart()
        {
            decimal number;

            var data = _context.tblEvents
                     .GroupBy(c => c.EventEndDate.Month).Select(g => new
                     {
                         Month = g.Key,
                         Count = g.Count()
                     });


            decimal?[] target = new decimal?[12];
            foreach (var u in data)
            {
                target[u.Month-1] = u.Count;
            }



            ChartModel _chart = new ChartModel();
            _chart.labels = monthStrings;
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Meetings",
                data = target,
                backgroundColor = GetRandomColour(12,"backGround"),
                borderColor = GetRandomColour(12,"borderColor"),
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
        }

        private static readonly Random rand = new Random();

        private string[] GetRandomColour(int number, string colorType)
        {
            var backgroundColor = new string[number];
            var myBackgroundColors = new string[]
            {
                "rgba(255, 99, 132, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(255, 159, 64, 0.2)",
                "rgba(255, 99, 132, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(255, 159, 64, 0.2)"
            };
            var borderColor = new string[]
            {
                "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)", "rgba(153, 102, 255, 1)", "rgba(255, 159, 64, 1)",
                "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)", "rgba(153, 102, 255, 1)", "rgba(255, 159, 64, 1)"
            };

            for (int i = 0; i < number; i++)
            {
                if (colorType == "backGround")
                {
                    backgroundColor[i] = myBackgroundColors[i];
                } else if (colorType == "borderColor")
                {
                    backgroundColor[i] = borderColor[i];
                }
            }
            return backgroundColor;
        }
        
    }
}
