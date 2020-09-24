using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingSystem.Models;
using VotingSystem.Service;
using VotingSystem.Utils;

namespace VotingSystem.Controllers
{
    /*[Route("")]*/
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly UserService _userService;

        public LoginController(ILogger<LoginController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        public JSONResult login([FromBody]User user)
        {
            if(user.Username.Length==0 || user.Password.Length==0)
            {
                return JSONResult.ErrorMsg("");
            }
            SessionUser suser = _userService.CheckUser(user.Username, user.Password);
            if (suser == null)
            {
                return JSONResult.ErrorMsg("用户名或密码错误");
            }
            HttpContext.Session.Set("user", suser);
            if(suser.Type == true)
            {
                return JSONResult.Ok(true);
            }
            return JSONResult.Ok(false);
        }

        [HttpGet("logout")]
        public JSONResult logout()
        {
            HttpContext.Session.Remove("user");
            return JSONResult.Ok();
        }
    }
}