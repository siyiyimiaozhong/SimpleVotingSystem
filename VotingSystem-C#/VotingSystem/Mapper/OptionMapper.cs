using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Mapper
{
    public interface OptionMapper
    {
        public int SaveOptions(int id, List<Option> options);
        public List<Option> FindOptionByVid(int id);
        public int DeleteOptionsByVid(int id);
    }
}
