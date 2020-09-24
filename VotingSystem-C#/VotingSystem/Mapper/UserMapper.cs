using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Mapper
{
    public interface UserMapper
    {
        public List<User> FindAll(PageObject<User> page);
        public User FindUserByUsername(string username);
        public int SaveUser(User user);
        public int GetUserNum();
        public int UpdateUser(User user);
        public int DeleteUser(int id);
        public int DeleteUsers(string delStr);
        public List<User> FindAllBySearch(PageObject<User> page);
    }
}
