using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class MockReportRepository : IReportRepository
    {
        private List<Report> reportList;

        public MockReportRepository()
        {
            reportList = new List<Report>()
                {
                    new Report() {Id = 1, Title = "Cos nie dziala", Assigness = "Mathew", Priority = "Important",  Status = "TODO"},
                    new Report() {Id = 2, Title = "Tralalalallalalalalala", Assigness = "Mathew", Priority = "Important",  Status = "DONE"},
                    new Report() {Id = 3, Title = "Opisssssssssssssssss", Assigness = "Mathew", Priority = "Important",  Status = "TODO"}
                };
        }

        public IEnumerable<Report> GetAllReports()
        {
            return reportList;
        }

        public Report GetReport(int Id)
        {
            return reportList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
