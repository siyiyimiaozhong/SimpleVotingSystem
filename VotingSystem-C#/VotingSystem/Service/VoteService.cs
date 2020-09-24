using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Service
{
    public interface VoteService
    {
        public int AddVote(VotingTheme vote);
        public PageObject<VotingTheme> FindAll(PageObject<VotingTheme> page);
        public int UpdateVote(VotingTheme vote);
        int DeleteVote(int id);
        int DeleteVotes(string delStr);
        int GetVoteNum();
        int GetTackingNum();
        VotingTheme VoteDetail(VotingTheme vote);
    }
}