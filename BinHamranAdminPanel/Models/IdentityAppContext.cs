using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BinHamranAdminPanel.Models
{
    public class IdentityAppContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<DataBaseName> DbNames { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbQuery<EntryModel> Entries { get; set; } 
        public DbQuery<BranchModel> Branchesm { get; set; } 
        public DbQuery<AccountModel> Accounts { get; set; } 
    }
}
