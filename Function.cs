using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using CC2020_Lambda_Payslip.Data;
using CC2020_Lambda_Payslip.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CC2020_Lambda_Payslip
{
    public class Function
    {
        private const string BucketName = "cc2020-payslips";

        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var response = await PutS3Object("cc2020-timesheets", $"{input}.txt", "This is just something to put into the bucket");

            return input?.ToUpper();
        }

        public static async Task<bool> PutS3Object(string bucket, string key, string content)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = $"{key}/{DateTime.UtcNow.ToString("s", CultureInfo.CreateSpecificCulture("de-DE"))}",
                        ContentBody = content
                    };
                    var response = await client.PutObjectAsync(request);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in PutS3Object:" + ex.Message);
                return false;
            }
        }

        public void RetrieveDB()
        {
            PayService _payservice = new PayService();

            using (var context = new TimesheetsContext())
            {
            };
        }
    }
}
