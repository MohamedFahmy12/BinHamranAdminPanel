using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinHamranAdminPanel.Models
{
  public class UserModel
  {
    public string Id { get; set; }
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Role { get; set; }

    public DateTime CreationDate { get; set; }

    public int Count { get; set; }

    public IEnumerable<AppRole> AppRoles { get; set; }
  }
}
