using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Service;
using VotingSystem.Utils;
using Microsoft.AspNetCore.Http;

namespace VotingSystem.Controllers
{
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        
        public UserController(UserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public JSONResult Register([FromBody]User user)
        {
            if(user == null)
            {
                return JSONResult.ErrorMsg("注册失败");
            }
            int row = _userService.SaveUser(user);
            if (row == 0) return JSONResult.ErrorMsg("注册失败.");
            return JSONResult.Ok();
        }

        [HttpGet("num")]
        public int GetNum()
        {
            return _userService.GetUserNum();
        }

        [HttpPost("findAll")]
        public Object FindAll([FromBody]PageObject<User> page)
        {
            return _userService.FindAll(page);
        }

        [HttpPost("update")]
        public JSONResult UpdateUser([FromBody]User user)
        {
            int row = _userService.UpdateUser(user);
            if(row==0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("delete")]
        public JSONResult DeleteUser([FromBody]User user)
        {
            int row = _userService.DeleteUser(user.Id);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("deleteUsers")]
        public JSONResult DeleteUsers([FromBody]List<User> users)
        {
            string delStr = "(";
            int len = users.Count();
            for(int i=0;i<len;i++)
            {
                if (i != 0) delStr += ',';
                delStr += users[i].Id;
            }
            delStr += ')';
            Console.WriteLine(delStr);
            int row = _userService.DeleteUsers(delStr);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }
    }
}
