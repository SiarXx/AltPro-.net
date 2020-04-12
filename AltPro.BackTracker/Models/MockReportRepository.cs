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
                    new Report() {Id = 1, Title = "Cos nie dziala", Assigness = "Mathew", Priority = "Important",  Status = "TODO", Time = "1 days ago"},
                    new Report() {Id = 2, Title = "Tralalalallalalalalala", Assigness = "John", Priority = "Important",  Status = "DONE", Time = "3 day ago"},
                    new Report() {Id = 3, Title = "Opisssssssssssssssss", Assigness = "Jack", Priority = "Important",  Status = "TODO", Time = "2 days ago"}
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
