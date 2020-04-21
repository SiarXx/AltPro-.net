using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public interface IReportRepository
    {
        Report GetReport(int Id);
        IEnumerable<Report> GetAllReports();
    }
}
