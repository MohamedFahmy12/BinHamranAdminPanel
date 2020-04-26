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

namespace BinHamranAdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDataController : ControllerBase
    {
        private ApplicationSettings appSettings;
        private IdentityAppContext context;

        public ReportDataController(IOptions<ApplicationSettings> appSettings, IdentityAppContext context)
        {
            this.appSettings = appSettings.Value;
            this.context = context;
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