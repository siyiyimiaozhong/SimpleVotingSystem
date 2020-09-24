using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;
using VotingSystem.Service;
using VotingSystem.Utils;

namespace VotingSystem.Controllers
{
    [Route("poll")]
    public class PollController: ControllerBase
    {
        private readonly PollService _pollService;

        public PollController(PollService pollService)
        {
            this._pollService = pollService;
        }
        
        [HttpPost("add")]
        public JSONResult Add([FromBody] Poll poll)
        {
            if (poll == null)
            {
                return JSONResult.ErrorMsg("投票失败");
            }
            User user = HttpContext.Session.Get<User>("user");
            poll.Uid = user.Id;
            Console.WriteLine(poll.Uid+" "+poll.Vid+"  " + poll.Oids[0]);
            int row = _pollService.SavePoll(poll);
            if (row == 0) return JSONResult.ErrorMsg("投票失败.");
            return JSONResult.Ok();
        }

        [HttpPost("check")]
        public JSONResult Check([FromBody] Poll poll)
        {
            if (poll == null)
            {
                return JSONResult.ErrorMsg("");
            }
            User user = HttpContext.Session.Get<User>("user");
            poll.Uid = user.Id;
            bool flag = _pollService.CheckPoll(poll);
            if (flag) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }
    }
}
