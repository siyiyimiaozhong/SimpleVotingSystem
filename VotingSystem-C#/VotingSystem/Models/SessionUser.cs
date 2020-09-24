using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingSystem.Models
{
    public class SessionUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
    }
}
