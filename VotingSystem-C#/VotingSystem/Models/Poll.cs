using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingSystem.Models
{
    public class Poll
    {
        public int Uid { get; set; }
        public int Vid { get; set; }
        public List<int> Oids { get; set; }
    }
}
