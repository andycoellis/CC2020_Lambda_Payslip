## COSC2626 Cloud Computing Assignment 02

## AWS Lambda Function

*RMIT University Melbourne*
<br>**group members:**
> Andrew Ellis - s3747746
<br>Shrey Parekh - s3710669

### Lambda Features
**C# Script for Payslip Automation:** 
>+ *Pay Service*
>+ *Weekly Payslip Template*
>+ *Push Weekly Payslip Object into S3 Bucket*

#### Notes
>This C# Script utilises Net Core's Entity Framework to generate all relevant payslips of the given week. The code is set to be called by an **AWS Lambda** in which on a trigger will generate a Weekly Payslip object of an employee and send, as a **JSON** into an AWS S3 Bucket.

#### System Requirements

**ASP.NET Core**
- .NET Core SDK 3.1.202
- .Net Core Runtime 3.1.4

- VisualStudio Version 8.5.6 (build 11)

#### Dependencies
**Project SDK**
- Microsoft.NET.Sdk

**Frameworks**
- Microsoft.NETCore.App (3.1.0)

**NuGet** *packages*
- Amazon.Lambda.Core
- Amazon.Lambda.Logging.AspNetCore
- Amazon.Lambda.Serialization.SystemTextJson
- AWSSDK.Core
- AWSSDK.S3
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer
- AWSSDK.S3

#### Application Architecture
The application was with ASP.NET Core Lambda
