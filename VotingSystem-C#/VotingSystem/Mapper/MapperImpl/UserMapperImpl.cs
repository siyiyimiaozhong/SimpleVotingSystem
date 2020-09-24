using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.conf;
using VotingSystem.Models;

namespace VotingSystem.Mapper.MapperImpl
{
    public class UserMapperImpl : UserMapper
    {
        private readonly ILogger<UserMapperImpl> _logger;

        public UserMapperImpl(ILogger<UserMapperImpl> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根据Id删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回影响行数</returns>
        public int DeleteUser(int id)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"DELETE FROM userInfo WHERE id=@id";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    row = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("UserMappImpl类DeleteUser()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 删除选中的所有用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>返回影响行数</returns>
        public int DeleteUsers(string delStr)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"DELETE FROM userInfo WHERE id in "+delStr;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    row = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("UserMappImpl类DeleteUsers()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<User> FindAll(PageObject<User> page)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,username,name,sex,age,address,phone,email,createTime,type FROM userInfo WHERE type<>1 ORDER BY id DESC  OFFSET @start ROWS FETCH NEXT @size ROWS ONLY;";
                    SqlParameter[] pms = {
                        new SqlParameter("@start",(page.CurrentPage-1)*page.PageSize),
                        new SqlParameter("@size",page.PageSize)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while(reader.Read())
                    {
                        User user = new User();
                        user = GetUser(user, reader);
                        users.Add(user);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("UserMappImpl类FindAll()方法异常：\n" + e.Message);
            }
            return users;
        }

        public List<User> FindAllBySearch(PageObject<User> page)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,username,name,sex,age,address,phone,email,createTime,type FROM userInfo WHERE type<>1 AND name like N'%"+page.Search+"%' ORDER BY id DESC  OFFSET @start ROWS FETCH NEXT @size ROWS ONLY;";
                    SqlParameter[] pms = {
                        new SqlParameter("@start",(page.CurrentPage-1)*page.PageSize),
                        new SqlParameter("@size",page.PageSize),
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User();
                        user = GetUser(user, reader);
                        users.Add(user);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("UserMappImpl类FindAll()方法异常：\n" + e.Message);
            }
            return users;
        }

        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User FindUserByUsername(string username)
        {
            User user = new User();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,username,password,name,sex,age,address,phone,email,createTime,type FROM userInfo WHERE username=@username";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("@username", username));
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user.Password = reader["password"].ToString();
                        user = GetUser(user, reader);
                        reader.Close();
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("UserMappImpl类FindUserByUsername()方法异常：\n" + e.ToString());
            }
            return user;
        }

        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <returns></returns>
        public int GetUserNum()
        {
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT COUNT(1) FROM userInfo WHERE type<>1";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    num = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("UserMappImpl类GetUserNum()方法异常：\n" + e.ToString());
            }
            return num;
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="user">需要新增的用户</param>
        /// <returns>返回影响行数</returns>
        public int SaveUser(User user)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"INSERT INTO userInfo(username,password,name,sex,age,address,phone,email) VALUES(@username,@password,@name,@sex,@age,@address,@phone,@email)";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    SqlParameter[] pms = {
                        new SqlParameter("@username",user.Username),
                        new SqlParameter("@password",user.Password),
                        new SqlParameter("@name",user.Name),
                        new SqlParameter("@sex",user.Sex),
                        new SqlParameter("@age",user.Age),
                        new SqlParameter("@address",user.Address),
                        new SqlParameter("@phone",user.Phone),
                        new SqlParameter("@email",user.Email)
                    };
                    connection.Open();
                    cmd.Parameters.AddRange(pms);
                    row = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("UserMappImpl类SaveUser()方法异常：\n" + e.ToString());
            }
            return row;
        }

        /// <summary>
        /// 修改数据库中用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns>返回影响行数</returns>
        public int UpdateUser(User user)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"UPDATE userInfo SET name=@name,sex=@sex,age=@age,address=@address,phone=@phone,email=@email WHERE id=@id";
                    SqlParameter[] pms = {
                        new SqlParameter("@name",user.Name),
                        new SqlParameter("@sex",user.Sex),
                        new SqlParameter("@age",user.Age),
                        new SqlParameter("@address",user.Address),
                        new SqlParameter("@phone",user.Phone),
                        new SqlParameter("@email",user.Email),
                        new SqlParameter("@id",user.Id)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    row = (int)cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("UserMappImpl类UpdateUser()方法异常：\n" + e.ToString());
            }
            return row;
        }

        private User GetUser(User user,SqlDataReader reader)
        {
            user.Id = Convert.ToInt32(reader["id"]);
            user.Username = reader["username"].ToString();
            user.Name = reader["name"].ToString();
            user.Sex = reader["sex"].ToString();
            user.Age = Convert.ToInt32(reader["age"]);
            user.Address = reader["address"].ToString();
            user.Phone = reader["phone"].ToString();
            user.Email = reader["email"].ToString();
            user.Type = Convert.ToInt32(reader["type"])==1 ? true:false;
            user.CreateTime = Convert.ToDateTime(reader["createTime"]);
            return user;
        }
    }
}
