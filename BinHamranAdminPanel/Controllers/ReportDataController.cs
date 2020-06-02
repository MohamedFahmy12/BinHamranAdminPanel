using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BinHamranAdminPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.Json;
using System.IO;
using Stimulsoft.Report;
using Newtonsoft.Json.Linq;
using Stimulsoft.Report.Mvc;

namespace BinHamranAdminPanel.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReportDataController : Controller
  {
    private ApplicationSettings appSettings;
    private IdentityAppContext context;

    public ReportDataController(IOptions<ApplicationSettings> appSettings, IdentityAppContext context)
    {
      this.appSettings = appSettings.Value;
      this.context = context;
    }
    [HttpGet]
    public string GetFile(int id)

    {
      StiReport report = new StiReport();
      report.Dictionary.DataStore.Clear();
      report.Load(Path.GetFullPath("SimpleList.mrt"));
      report.Dictionary.Variables["id"].ValueObject = id;
      string data = report.SaveToJsonString();

      return data;
    }
    [HttpGet]
    [Route("getReportForDesigner")]

    public string getReportForDesigner(string reportName)
    {
      var path = StiNetCoreHelper.MapPath(this, "/Reports/" + reportName + ".mrt");
      StreamReader rd = new StreamReader(path);
      string data = rd.ReadToEnd();
      rd.Close();
      return data;
    }

    [HttpPost]
    [Route("SaveFile")]
    public string SaveFile([FromBody] DemoData jsonString)
    {

      var reportName = jsonString.fileName;
      try
      {
        string filePath = StiNetCoreHelper.MapPath(this,"/Reports/" + reportName + ".mrt");
        StreamWriter wr = new StreamWriter(filePath);
        wr.Write(jsonString.data);
        wr.Close();
        return "Report Saved Successfully";
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }


    [HttpPost]
    [Route("GetDataSource")]
    public void GetDataSource()
    {


      try
      {
        var command = (CommandJson)new DataContractJsonSerializer(typeof(CommandJson)).ReadObject(HttpContext.Request.BodyReader.AsStream());
        Result result = new Result();

        if (command.Database == "MS SQL")
        {
          result = MSSQLAdapter.Process(command);
        }
        //if (command.Database == "PostgreSQL") result = PostgreSQLAdapter.Process(command);

        var serializer = new DataContractJsonSerializer(typeof(Result));

        serializer.WriteObject(HttpContext.Response.BodyWriter.AsStream(), result);
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        HttpContext.Response.Headers.Add("Cache-Control", "no-cache");

        HttpContext.Response.Body.Flush();
        HttpContext.Response.Clear();
      }
      catch { }


    }
    [HttpGet]
    [Route("GetDbNames")]
    public List<DatabaseModel> GetDbNames()
    {
      var dbNames = context.DbNames.ToList();
      var Dbs = new List<DatabaseModel>();
      foreach (var item in dbNames)
      {
        var Db = new DatabaseModel();

        Db.DatabaseNameId = item.DatabaseNameId;
        Db.DbName = item.DbArabicName;
        Dbs.Add(Db);

      }

      //var Db = new DatabaseModel();
      //var dbNames =  context.DbNames.ToList();
      //foreach (var item in dbNames)
      //{

      //    Db.DatabaseNameId = item.DatabaseNameId;
      //    Db.DbName = item.DbArabicName;
      //    Dbs.Add(Db);
      //};
      return Dbs;

    }
    [HttpGet]
    [Route("GetCompanyBranches")]
    public IQueryable<BranchModel> GetCompanyBranches(string dbIds)
    {

      var parameter = new SqlParameter
      {
        ParameterName = "DatabaseID",
        Value = dbIds
      };
      var Branches = context.Branchesm.FromSqlRaw("EXEC Get_CompanyBranches @DatabaseID", parameter);
      return Branches;
    }
    [HttpGet]
    [Route("GetEntries")]
    public IQueryable<EntryModel> GetEntries(string dbIds)
    {
      var parameter = new SqlParameter
      {
        ParameterName = "DatabaseID",
        Value = dbIds
      };
      var Entries = context.Entries.FromSqlRaw("EXEC Get_EntryType @DatabaseID", parameter);
      return Entries;
    }
    [HttpGet]
    [Route("GetAccounts")]
    public IQueryable<AccountModel> GetAccounts(string dbIds)
    {
      var parameter = new SqlParameter
      {
        ParameterName = "DatabaseID",
        Value = dbIds
      };
      var Accounts = context.Accounts.FromSqlRaw("EXEC Get_ACCOUNTS @DatabaseID", parameter);
      return Accounts;
    }
  }


}
