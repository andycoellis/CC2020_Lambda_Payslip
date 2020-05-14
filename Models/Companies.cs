using System;
using System.Collections.Generic;

namespace CC2020_Lambda_Payslip.Models
{
    public partial class Companies
    {
        public Companies()
        {
            PayAgreements = new HashSet<PayAgreements>();
            Timesheets = new HashSet<Timesheets>();
        }

        public long Abn { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual ICollection<PayAgreements> PayAgreements { get; set; }
        public virtual ICollection<Timesheets> Timesheets { get; set; }
    }
}
