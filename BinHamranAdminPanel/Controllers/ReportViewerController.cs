using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BinHamranAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BinHamranAdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportViewerController : Controller
    {
        private readonly ApplicationSettings appSettings;
        private IdentityAppContext context;

        public ReportViewerController(IOptions<ApplicationSettings> appSettings
            , IdentityAppContext context)
        {
            this.appSettings = appSettings.Value;
            this.context = context;
        }

        //MonthlyAnalisisForEntriesModel
        [HttpPost]
        [Route("MonthlyAnalisisForEntriesModel")]
        public string MonthlyAnalisisForEntriesModel([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string entryID = data.GetValue("entryID").ToString();
            string databaseID = data.GetValue("databaseID").ToString();

            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this,"Reports/MonthlyAnalisisForEntriesModel.mrt");
            report.Load(path);
            report["param_1"] = startDate;
            report["@STARTDATE"] = startDate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@EntryID"] = entryID;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        //Opening Entries
        [HttpPost]
        [Route("OpeningEntries")]
        public string OpeningEntries([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string databaseID = data.GetValue("databaseID").ToString();

            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "Reports/OpeningEntries.mrt");
            report.Load(path);
            report["param_1"] = startDate;
            report["@STARTDATE"] = startDate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        //Monthly Analysis For Accounts
        [HttpPost]
        [Route("MonthlyAnalisisForAccounts")]
        public string MonthlyAnalisisForAccounts([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string AccountID = data.GetValue("AccountID").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "Reports/MonthlyAnalisisForAccounts.mrt");
            report.Load(path);
            
            report["param_1"] = startDate;
            report["@STARTDATE"] = startDate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@ACCOUNTID"] = AccountID;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
    }
}
