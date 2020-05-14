using System;
using System.Collections.Generic;

namespace CC2020_Lambda_Payslip.Models
{
    public partial class Timesheets
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Break { get; set; }
        public string EmployeeId { get; set; }
        public long CompanyAbn { get; set; }

        public virtual Companies CompanyAbnNavigation { get; set; }
        public virtual AspNetUsers Employee { get; set; }
    }
}
