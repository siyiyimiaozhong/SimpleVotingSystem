using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Mapper
{
    public interface VoteMapper
    {
        public int SaveVoteTheme(VotingTheme vote);
        public List<VotingTheme> FindAll(PageObject<VotingTheme> page);
        public List<VotingTheme> FindAllBySearch(PageObject<VotingTheme> page);
        public int FindNum();
        public int FindNumBySearch(PageObject<VotingTheme> page);
        public int UpdateVote(VotingTheme vote);
        int DeleteVote(int id);
        int DeleteVotes(string delStr);
        int GetTackingNum(string time);
    }
}
