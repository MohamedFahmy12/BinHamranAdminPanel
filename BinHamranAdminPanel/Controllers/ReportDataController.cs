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
        public IQueryable<EntryModel> GetAccounts(string dbIds)
        {
            var parameter = new SqlParameter
            {
                ParameterName = "DatabaseID",
                Value = dbIds
            };
            var Entries = context.Entries.FromSqlRaw("EXEC ACCOUNTS @DatabaseID", parameter);
            return Entries;
        }
    }


}