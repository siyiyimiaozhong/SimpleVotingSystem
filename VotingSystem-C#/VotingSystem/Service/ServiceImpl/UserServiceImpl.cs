using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Mapper;
using VotingSystem.Models;
using VotingSystem.Utils;

namespace VotingSystem.Service.ServiceImpl
{
    public class UserServiceImpl : UserService
    {
        private readonly UserMapper _userMapper;

        public UserServiceImpl(UserMapper userMapper)
        {
            _userMapper = userMapper;
        }
        
        /// <summary>
        /// 校验登录用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>0：验证失败 1：普通用户 2：管理员</returns>
        public SessionUser CheckUser(string username, string password)
        {
            User user = _userMapper.FindUserByUsername(username);
            password = MD5Util.MD5Encrypt(password);
            if (user == null || !password.Equals(user.Password))
            {
                return null;
            }
            SessionUser suser = new SessionUser();
            suser.Id = user.Id;
            suser.Username = user.Username;
            suser.Name = user.Name;
            suser.Type = user.Type;
            return suser;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteUser(int id)
        {
            return _userMapper.DeleteUser(id);
        }

        /// <summary>
        /// 删除ids中所有对应用户信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteUsers(string delStr)
        {
            return _userMapper.DeleteUsers(delStr);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>zhi
        public List<User> FindAll(PageObject<User> page)
        {
            if (page.Search == null || page.Search == "") return _userMapper.FindAll(page);
            return _userMapper.FindAllBySearch(page);
        }

        /// <summary>
        /// 获取用户总数
        /// </summary>
        /// <returns></returns>
        public int GetUserNum()
        {
            return _userMapper.GetUserNum();
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int SaveUser(User user)
        {
            user.Password = MD5Util.MD5Encrypt(user.Password);
            return _userMapper.SaveUser(user);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateUser(User user)
        {
            return _userMapper.UpdateUser(user);
        }
    }
}
