using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
  }
}
