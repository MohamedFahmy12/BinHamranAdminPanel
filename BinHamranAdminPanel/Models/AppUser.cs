using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinHamranAdminPanel.Models
{
  public class AppUser : IdentityUser<int>
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Creationdate { get; set; }

    public virtual ICollection<UserLog> UserLogs { get; set; }

  }
}
