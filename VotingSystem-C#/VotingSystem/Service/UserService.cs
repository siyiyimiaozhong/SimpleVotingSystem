using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Models;

namespace VotingSystem.Service
{
    public interface UserService
    {
        public List<User> FindAll(PageObject<User> page);
        public SessionUser CheckUser(string username, string password);
        public int SaveUser(User user);
        public int GetUserNum();
        public int UpdateUser(User user);
        public int DeleteUser(int id);
        public int DeleteUsers(string detStr);
    }
}
