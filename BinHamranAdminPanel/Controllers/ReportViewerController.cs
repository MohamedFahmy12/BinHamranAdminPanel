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
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/MonthlyAnalisisForEntriesModel.mrt");
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
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/OpeningEntries.mrt");
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
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/MonthlyAnalisisForAccounts.mrt");
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

        [HttpPost]
        [Route("AccountTotalBalance")]
        public string AccountTotalBalance([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string AccountID = data.GetValue("AccountID").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/AccountTotalBalance.mrt");
            report.Load(path);
            report["Date"] = sdate;
            report["@STARTDATE"] = sdate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@ACCOUNTID"] = AccountID;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }

        [HttpPost]
        [Route("GeneralDaily")]
        public string GeneralDaily([FromBody] JObject data)
        {

            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/GeneralDaily.mrt");
            report.Load(path);
            report["param_1"] = startDate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("ConsumptionOfDepreciation")]
        public string ConsumptionOfDepreciation([FromBody] JObject data)
        {

            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            string AccountID = data.GetValue("AccountID").ToString();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/ConsumptionOfDepreciation.mrt");

            report.Load(path);
            report["FromDate"] = startDate;
            report["ToDate"] = endDate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@ACCOUNTID"] = AccountID;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("ProvisionForDepreciationOfFixedAssets")]
        public string ProvisionForDepreciationOfFixedAssets([FromBody] JObject data)
        {

            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            string AccountID = data.GetValue("AccountID").ToString();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/ProvisionForDepreciationOfFixedAssets.mrt");

            report.Load(path);
            report["FromDate"] = startDate;
            report["ToDate"] = endDate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@ACCOUNTID"] = AccountID;
            report["@DatabaseID"] = databaseID;


            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("Institutionfees")]
        public string Institutionfees([FromBody] JObject data)
        {
            var path = "";
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            short type = (short)data.GetValue("type");
            StiReport report = new StiReport();
            if (type == 1)
            {
                path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/InstitutionfeesBranches.mrt");
            }
            else
            {
                path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/InstitutionfeesMonths.mrt");
            }

            report.Load(path);
            report["FromDate"] = startDate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@Type"] = type;
            report["@DatabaseID"] = databaseID;



            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }

        [HttpPost]
        [Route("EntriesAnalysis")]
        public string EntriesAnalysis([FromBody] JObject data)
        {
            var path = "";
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            string entryID = data.GetValue("entryID").ToString();
            string databaseID = data.GetValue("databaseID").ToString();
            short type = (short)data.GetValue("type");
            StiReport report = new StiReport();
            if (type == 1)
            {
                path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/InstitutionfeesBranches.mrt");
            }
            else
            {
                path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/InstitutionfeesMonths.mrt");
            }

            report.Load(path);
            report["FromDate"] = startDate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@CompanyBranchID"] = companyBranchCode;
            report["@EntryID"] = entryID;
            report["@DatabaseID"] = databaseID;



            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("InstitutionTrialBalance")]
        public string InstitutionTrialBalance([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string databaseID = data.GetValue("databaseID").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/InstitutionTrialBalance.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["@STARTDATE"] = sdate;
            report["@DatabaseID"] = databaseID;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }

        [HttpPost]
        [Route("BranchesTrialBalance")]
        public string BranchesTrialBalance([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/BranchesTrialBalance.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["@STARTDATE"] = sdate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("LastDurationGoods")]
        public string LastDurationGoods([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/LastDurationGoods.mrt");
            report.Load(path);
            report["FromDate"] = startDate;
            report["@STARTDATE"] = sdate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("DepreciationOfFixedAssets")]
        public string DepreciationOfFixedAssets([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/DepreciationOfFixedAssets.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("TotalExclusionOfFixedAssets")]
        public string TotalExclusionOfFixedAssets([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/TotalExclusionOfFixedAssets.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("FixedAssetsAdditions")]
        public string FixedAssetsAdditions([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/FixedAssetsAdditions.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }
        [HttpPost]
        [Route("FixedAssetsOfInstitution")]
        public string FixedAssetsOfInstitution([FromBody] JObject data)
        {
            string startDate = data.GetValue("startDate").ToString();
            DateTime sdate = ChangeDateFormat(startDate);
            string endDate = data.GetValue("endDate").ToString();
            DateTime edate = ChangeDateFormat(endDate);
            string databaseID = data.GetValue("databaseID").ToString();
            string companyBranchCode = data.GetValue("companyBranchCode").ToString();
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, "C:/MohamedFahmy/BinHamranAdminPanelFront/src/reports/FixedAssetsOfInstitution.mrt");
            report.Load(path);
            report["FromDate"] = sdate;
            report["ToDate"] = edate;
            report["@STARTDATE"] = sdate;
            report["@ENDDATE"] = edate;
            report["@DatabaseID"] = databaseID;
            report["@CompanyBranchID"] = companyBranchCode;
            var dbMS_SQL = (StiSqlDatabase)report.Dictionary.Databases["MS SQL"];
            dbMS_SQL.ConnectionString = appSettings.Report_Connection;
            report.Render(false);
            return report.SaveDocumentJsonToString();
        }

        public static DateTime ChangeDateFormat(string FromDate)
        {
            DateTime date = DateTime.ParseExact(FromDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            string fdate = date.ToString("yyyy-M-d");
            return Convert.ToDateTime(fdate);
        }

    }
}
