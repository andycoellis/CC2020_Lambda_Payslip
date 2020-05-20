using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using CC2020_Lambda_Payslip.Services;
using CC2020_Lambda_Payslip.Data;
using CC2020_Lambda_Payslip.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CC2020_Lambda_Payslip
{
    public class Function
    {
        private const string BucketName = "cc2020-drop-box";

        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            List<WeeklyPayslip> payslips = RetrievePayslip();

            var response = await PutS3Object(payslips);

            var reply = response ? $"{input} complete" : $"{input} failure";

            return reply?.ToUpper();
        }

        public async Task FunctionInit()
        {
            List<WeeklyPayslip> payslips = RetrievePayslip();

            await PutS3Object(payslips);

        }

        public static async Task<bool> PutS3Object(List<WeeklyPayslip> payslips)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    LambdaLogger.Log("Connection Open to S3 Client");


                    foreach (var pay in payslips)
                    {
                        var payslip = new { Details = new[] { pay } };

                        var output = JsonConvert.SerializeObject(payslip);

                        LambdaLogger.Log($"\n\n{pay.EmpId}\n\n{output}");

                        var request = new PutObjectRequest
                        {                           
                            BucketName = BucketName,
                            Key = $"{pay.EmpId}-{DateTime.UtcNow.ToString("s", CultureInfo.CreateSpecificCulture("de-DE"))}.json",
                            ContentBody = output
                        };
                        var response = await client.PutObjectAsync(request);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in PutS3Object:" + ex.Message);
                return false;
            }
        }

        public List<WeeklyPayslip> RetrievePayslip()
        {

            var _payservice = new PayService();
            var payslips_all = new List<WeeklyPayslip>();

            using (var context = new TimesheetsContext())
            {
                LambdaLogger.Log("Context Retrieval");

                var payAgreements = context.PayAgreements;

                var timesheets = context.Timesheets
                                    .Where(x => x.Date >= MinDate(false)
                                    && x.Date < MaxDate(false));

                foreach (var payAgr in payAgreements)
                {
                    var empTimesheet = timesheets
                                    .Where(x => x.EmployeeId == payAgr.EmployeeId
                                    && x.CompanyAbn == payAgr.CompanyAbn).ToList();

                    var employee = context.AspNetUsers
                                    .Include(x => x.Payslips)
                                    .SingleOrDefault(x => x.Id == payAgr.EmployeeId);

                    var company = context.Companies
                                    .SingleOrDefault(x => x.Abn == payAgr.CompanyAbn);

                    if (empTimesheet.Count > 0)
                    {
                        try
                        {
                            var payslip_print = _payservice.GetEmpoyeeTimesheet(employee, company, empTimesheet, payAgr);

                            payslips_all.Add(payslip_print);

                            var payslip_database = new Payslips()
                            {
                                WeekBegininning = MinDate(false),
                                GrossPay = Convert.ToDouble(payslip_print.Pay),
                                PayYtd = Convert.ToDouble(payslip_print.PayYTD),
                                Tax = Convert.ToDouble(payslip_print.Tax),
                                BaseHours = Convert.ToDouble(payslip_print.BaseHours),
                                SatHours = Convert.ToDouble(payslip_print.SatHours),
                                SunHours = Convert.ToDouble(payslip_print.SunHours),
                                CompanyAbn = Convert.ToInt64(payslip_print.CompanyABN),
                                EmployeeId = payslip_print.EmpId
                            };

                            context.Payslips.Add(payslip_database);
                        }
                        catch (Exception e)
                        {
                            LambdaLogger.Log(e.Message);
                        }
                    }
                }
                LambdaLogger.Log("Save Changes to Context");

                context.SaveChanges();
            };

            return payslips_all;
        }

        /// <summary>
        /// Return the minimum date to filter timesheets,
        /// true = financial year
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public DateTime MinDate(bool financialYear)
        {
            if (!financialYear)
            {
                return DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek - 1)).Date;
            }

            return new DateTime(0, 0, DateTime.Now.Year);
        }

        /// <summary>
        /// Return the maximum date to filter timesheets,
        /// true = financial year
        /// </summary>
        /// <param name="financialYear"></param>
        /// <returns></returns>
        public DateTime MaxDate(bool financialYear)
        {
            if (!financialYear)
            {
                return DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek - 1)).AddDays(6).Date;
            }

            return new DateTime(0, 0, DateTime.Now.Year + 1);
        }

    }
}
