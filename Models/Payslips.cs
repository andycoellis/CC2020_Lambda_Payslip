using System;
using System.Collections.Generic;

namespace CC2020_Lambda_Payslip.Models
{
    public partial class Payslips
    {
        public int Id { get; set; }
        public DateTime WeekBegininning { get; set; }
        public double GrossPay { get; set; }
        public double PayYtd { get; set; }
        public double Tax { get; set; }
        public double BaseHours { get; set; }
        public double SatHours { get; set; }
        public double SunHours { get; set; }
        public string EmployeeId { get; set; }
        public long CompanyAbn { get; set; }

        public virtual Companies CompanyAbnNavigation { get; set; }
        public virtual AspNetUsers Employee { get; set; }
    }
}
