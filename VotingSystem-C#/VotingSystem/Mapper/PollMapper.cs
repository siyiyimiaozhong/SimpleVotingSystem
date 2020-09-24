using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Mapper
{
    public interface PollMapper
    {
        int SavePoll(Poll poll);
        Poll FindPollByUidAndVid(Poll poll);
        int FindNumByVidAndOid(int id1, int id2);
    }
}
