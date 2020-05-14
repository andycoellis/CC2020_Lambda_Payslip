using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CC2020_Lambda_Payslip.Models;

namespace CC2020_Lambda_Payslip.Services
{
    /// <summary>
    /// Documentation of an employess weekly payslip
    /// </summary>
    public readonly struct WeeklyPayslip
    {
        public WeeklyPayslip(
                            AspNetUsers employee,
                            Companies company,
                            DateTime weekBeginning,
                            double pay,
                            double payYTD,
                            double baseHours,
                            double satHours,
                            double sunHours
                            )
        {
            EmpId = $"{employee.Id}";
            EmployeeName = employee.Name;
            CompanyName = company.CompanyName;
            CompanyABN = $"{company.Abn}";
            PayPeriod = $"" +
                $"{weekBeginning.ToString("d", CultureInfo.CreateSpecificCulture("es-ES"))} - " +
                $"{weekBeginning.AddDays(7).ToString("d", CultureInfo.CreateSpecificCulture("es-ES"))}";
            Pay = $"{pay}";
            PayYTD = $"{payYTD}";
            Tax = $"{pay * TaxRates.LOW}";
            BaseHours = $"{baseHours}";
            SatHours = $"{satHours}";
            SunHours = $"{sunHours}";
            ReceiverAddress = $"{employee.Email}";
            SenderAddress = $"{company.Email}";
        }

        public string EmpId { get; }
        public string EmployeeName { get; }
        public string CompanyName { get; }
        public string CompanyABN { get; }
        public string PayPeriod { get; }
        public string Pay { get; }
        public string PayYTD { get; }
        public string Tax { get; }
        public string BaseHours { get; }
        public string SatHours { get; }
        public string SunHours { get; }
        public string SenderAddress { get; }
        public string ReceiverAddress { get; }

    }
    public class PayService
    {
        ///<summary>Retrieve the employees weekly timesheet</summary>>
        public WeeklyPayslip GetEmpoyeeTimesheet(
                                                AspNetUsers employee,
                                                Companies company,
                                                List<Timesheets> employeeTimesheets,
                                                PayAgreements payAgreement
                                                )
        {

            //If timesheets are not of a weekly capacity throw exception
            if (employeeTimesheets.Capacity > 7 || employeeTimesheets.Capacity < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            //Helper method for calculating hours worked in a day
            Func<Timesheets, double> hoursWorked = x => (double)((x.EndTime - x.StartTime) - x.Break).TotalHours;

            var firstDate = employeeTimesheets[0].Date;

            var startOfWeek = firstDate.AddDays(-(int)firstDate.DayOfWeek);

            double baseHours = 0;
            double satHours = 0;
            double sunHours = 0;

            foreach (var ts in employeeTimesheets)
            {
                if (ts.Date.DayOfWeek == DayOfWeek.Saturday)
                {
                    satHours += hoursWorked(ts);
                }
                if (ts.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    sunHours += hoursWorked(ts);
                }
                else
                {
                    baseHours += hoursWorked(ts);
                }
            }

            //Employe pay is calculated as <(week hours) * payrate> + <weekend hours * (payrate * penalties)> 
            var pay = (baseHours * (double)payAgreement.PayRate) +
                      (satHours * ((double)payAgreement.PayRate * payAgreement.SaturdayRate)) +
                      (sunHours * ((double)payAgreement.PayRate * payAgreement.SundayRate));

            return new WeeklyPayslip
                (
                employee,
                company,
                startOfWeek,
                pay,
                GetYTD(employee.Id, company.Abn, employee.Payslips.ToList()),
                baseHours,
                satHours,
                sunHours
                );
        }

        /// <summary>
        /// Returns an employees gross pay in the given year, WARNING, there is no date input
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="cmpABN"></param>
        /// <param name="timesheets"></param>
        /// <returns></returns>
        public double GetYTD(string empID, long cmpABN, List<Payslips> payslips)
        {
            Func<Timesheets, double> hoursWorked = x => (double)((x.EndTime - x.StartTime) - x.Break).TotalHours;

            double grossPay = 0;

            foreach (var entry in payslips)
            {
                if (entry.CompanyAbn == cmpABN && entry.EmployeeId.ToUpper() == empID.ToUpper())
                {
                    grossPay += entry.GrossPay;
                }
            }
            return grossPay;
        }

    }
}

