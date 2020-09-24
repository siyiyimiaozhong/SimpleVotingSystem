using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Service
{
    public interface PollService
    {
        int SavePoll(Poll poll);
        bool CheckPoll(Poll poll);
    }
}
