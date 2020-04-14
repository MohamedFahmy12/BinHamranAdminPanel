using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinHamranAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;

namespace BinHamranAdminPanel.Controllers
{
  [Authorize(Roles = "SuperAdmin,Admin")]
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class ReportViewerController : ControllerBase
  {
    private readonly IOptions<ApplicationSettings> appSettings;
    private IdentityAppContext context;

    public ReportViewerController(IOptions<ApplicationSettings> appSettings
        , IdentityAppContext context)
    {
      this.appSettings = appSettings;
      this.context = context;
    }
  }
}
