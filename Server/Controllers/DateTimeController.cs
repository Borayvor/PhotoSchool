using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.ViewModels;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class DateTimeController : Controller
  {
    public string Get()
    {
      return JsonConvert.SerializeObject(new DateTimeViewModel
      {
        GetDateTime = DateTime.UtcNow
      });
    }
  }
}
