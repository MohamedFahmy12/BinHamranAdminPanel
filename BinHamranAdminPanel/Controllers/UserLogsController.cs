using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinHamranAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BinHamranAdminPanel.Controllers
{
  [Authorize(Roles = "SuperAdmin")]
  [Route("api/[controller]")]
  [ApiController]
  public class UserLogsController : ControllerBase
  {
    private  IdentityAppContext context;
    #region CTOR & Definitions


    public UserLogsController(IdentityAppContext context)
    {
      this.context = context;
    }
    #endregion

    [Route("~/api/UserLogs/GetHistory")]
    public IActionResult GetHistory()
    {
      var History = context.UserLogs.Select(z => new UserLogModel
      {
        UserName = z.User.UserName,
        OperationDate = z.OperationDate.ToString("d/MM/yyyy"),
        time = z.OperationDate.ToString("HH:mm:ss"),
        OperationName = z.OperationName,
        PageName = z.PageName,
        MobileView = z.MobileView,
        UserId = z.UserId,
        UserLogID = z.UserLogID



      });



      if (History == null)
      {
        return Ok(0);
      }

      return Ok(History);
    }
  }
}
