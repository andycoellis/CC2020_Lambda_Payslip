using System;
using System.Collections.Generic;

namespace CC2020_Lambda_Payslip.Models
{
    public partial class PayAgreements
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal PayRate { get; set; }
        public double SaturdayRate { get; set; }
        public double SundayRate { get; set; }
        public double PublicHolidayRate { get; set; }
        public string EmployeeId { get; set; }
        public long CompanyAbn { get; set; }

        public virtual Companies CompanyAbnNavigation { get; set; }
        public virtual AspNetUsers Employee { get; set; }
    }
}
