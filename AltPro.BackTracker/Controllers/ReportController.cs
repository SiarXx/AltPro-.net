using AltPro.BackTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Controllers
{
    public class ReportController : Controller
    {
        private IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public IActionResult ReportList()
        {
            var model = _reportRepository.GetAllReports();
            return View(model);
        }
    }
}
