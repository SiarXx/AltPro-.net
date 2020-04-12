using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AltPro.BackTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace AltPro.BackTracker.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IReportRepository _reportRepository;

        public HomeController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public IActionResult ReportList()
        {
            var model = _reportRepository.GetAllReports();
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
