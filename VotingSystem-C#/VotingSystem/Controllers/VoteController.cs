using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;
using VotingSystem.Service;
using VotingSystem.Utils;

namespace VotingSystem.Controllers
{
    [Route("vote")]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _voteService;

        public VoteController(VoteService voteService)
        {
            this._voteService = voteService;
        }

        [HttpGet("num")]
        public int GetVoteNum()
        {
            return _voteService.GetVoteNum();
        }

        [HttpGet("tackingNum")]
        public int GetTackingNum()
        {
            return _voteService.GetTackingNum();
        }

        [HttpPost("add")]
        public JSONResult AddVote([FromBody]VotingTheme vote)
        {
            int row = _voteService.AddVote(vote);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("findAll")]
        public PageObject<VotingTheme> FindAll([FromBody]PageObject<VotingTheme> page)
        {
            return _voteService.FindAll(page);
        }

        [HttpPost("update")]
        public JSONResult UpdateVote([FromBody]VotingTheme vote)
        {
            int row = _voteService.UpdateVote(vote);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("delete")]
        public JSONResult DeleteUser([FromBody] VotingTheme vote)
        {
            int row = _voteService.DeleteVote(vote.Id);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("deleteVotes")]
        public JSONResult DeleteUsers([FromBody] List<VotingTheme> votes)
        {
            string delStr = "(";
            int len = votes.Count();
            for (int i = 0; i < len; i++)
            {
                if (i != 0) delStr += ',';
                delStr += votes[i].Id;
            }
            delStr += ')';
            Console.WriteLine(delStr);
            int row = _voteService.DeleteVotes(delStr);
            if (row == 0) return JSONResult.ErrorMsg("");
            return JSONResult.Ok();
        }

        [HttpPost("detail")]
        public JSONResult Detail([FromBody] VotingTheme vote)
        {
            try
            {
                vote = _voteService.VoteDetail(vote);
            }
            catch(Exception e)
            {
                return JSONResult.ErrorMsg("");
            }
            return JSONResult.Ok(vote);
        }
    }
}
