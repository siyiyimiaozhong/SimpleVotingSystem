using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Mapper;
using VotingSystem.Models;

namespace VotingSystem.Service.ServiceImpl
{
    public class PollServiceImpl:PollService
    {
        private readonly PollMapper _pollMapper;

        public PollServiceImpl(PollMapper pollMapper)
        {
            this._pollMapper = pollMapper;
        }

        public bool CheckPoll(Poll poll)
        {
            Poll p = _pollMapper.FindPollByUidAndVid(poll);
            if (p.Oids.Count == 0) return false;
            return true;
        }

        public int SavePoll(Poll poll)
        {
            return _pollMapper.SavePoll(poll);
        }
    }
}
