using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.conf;
using VotingSystem.Models;

namespace VotingSystem.Mapper.MapperImpl
{
    public class VoteMapperImpl : VoteMapper
    {
        private readonly ILogger<VoteMapperImpl> _logger;

        public VoteMapperImpl(ILogger<VoteMapperImpl> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根据Id删除投票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回受影响的行数</returns>
        public int DeleteVote(int id)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"DELETE FROM votingTheme WHERE id=@id";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    row = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMappImpl类DeleteVote()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 删除选中的投票信息
        /// </summary>
        /// <param name="delStr"></param>
        /// <returns>返回影响行数</returns>
        public int DeleteVotes(string delStr)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"DELETE FROM votingTheme WHERE id in " + delStr;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    row = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMappImpl类DeleteVotes()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 分页查找全部投票
        /// </summary>
        /// <param name="page"></param>
        /// <returns>返回当前页的数据</returns>
        public List<VotingTheme> FindAll(PageObject<VotingTheme> page)
        {
            List<VotingTheme> votes = new List<VotingTheme>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,title,type,startTime,endTime,createTime FROM votingTheme ORDER BY id DESC  OFFSET @start ROWS FETCH NEXT @size ROWS ONLY;";
                    SqlParameter[] pms = {
                        new SqlParameter("@start",(page.CurrentPage-1)*page.PageSize),
                        new SqlParameter("@size",page.PageSize)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        VotingTheme vote = new VotingTheme();
                        vote = GetVote(vote, reader);
                        votes.Add(vote);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类FindAll()方法异常：\n" + e.Message);
            }
            return votes;
        }

        /// <summary>
        /// 通过关键字模糊分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <returns>返回符合条件的所有投票</returns>
        public List<VotingTheme> FindAllBySearch(PageObject<VotingTheme> page)
        {
            List<VotingTheme> votes = new List<VotingTheme>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,title,type,startTime,endTime,createTime FROM votingTheme WHERE title like N'%" + page.Search + "%'  ORDER BY id DESC  OFFSET @start ROWS FETCH NEXT @size ROWS ONLY;";
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
                        VotingTheme vote = new VotingTheme();
                        vote = GetVote(vote, reader);
                        votes.Add(vote);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类FindAllBySearch()方法异常：\n" + e.Message);
            }
            return votes;
        }

        /// <summary>
        /// 获取所有投票个数
        /// </summary>
        /// <returns></returns>
        public int FindNum()
        {
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT COUNT(*) FROM votingTheme;";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    num = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类FindNum()方法异常：\n" + e.ToString());
            }
            return num;
        }

        public int FindNumBySearch(PageObject<VotingTheme> page)
        {
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT COUNT(1) FROM votingTheme WHERE title like N'%" + page.Search + "%'";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    num = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类FindNumBySearch()方法异常：\n" + e.ToString());
            }
            return num;
        }

        /// <summary>
        /// 获取正在进行的任务数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetTackingNum(string time)
        {
            int num = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT COUNT(1) FROM votingTheme WHERE endTime>'"+time+"'";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    num = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类FindNum()方法异常：\n" + e.ToString());
            }
            return num;
        }

        /// <summary>
        /// 添加投票信息
        /// </summary>
        /// <param name="vote"></param>
        /// <returns>投票编号</returns>
        public int SaveVoteTheme(VotingTheme vote)
        {
            int id = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"INSERT INTO votingTheme(title,type,startTime,endTime) values(@title,@type,@startTime,@endTime) SELECT @@IDENTITY as Id";
                    SqlParameter[] pms = {
                        new SqlParameter("@title",vote.Title),
                        new SqlParameter("@type",vote.Type?1:0),
                        new SqlParameter("@startTime",vote.StartTime),
                        new SqlParameter("@endTime",vote.EndTime)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        id = Convert.ToInt32(reader["id"]);
                    }
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类SaveVoteTheme()方法异常：\n" + e.Message);
            }
            return id;
        }

        /// <summary>
        /// 修改投票信息
        /// </summary>
        /// <param name="vote"></param>
        /// <returns>返回影响行数</returns>
        public int UpdateVote(VotingTheme vote)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"UPDATE votingTheme SET title=@title,type=@type,startTime=@startTime,endTime=@endTime WHERE id=@id";
                    SqlParameter[] pms = {
                        new SqlParameter("@title",vote.Title),
                        new SqlParameter("@type",vote.Type?1:0),
                        new SqlParameter("@startTime",vote.StartTime),
                        new SqlParameter("@endTime",vote.EndTime),
                        new SqlParameter("@id",vote.Id)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    row = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation("VoteMapperImpl类UpdateVote()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 通过reader封装对象
        /// </summary>
        /// <param name="vote"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private VotingTheme GetVote(VotingTheme vote, SqlDataReader reader)
        {
            vote.Id = Convert.ToInt32(reader["id"]);
            vote.Title = reader["title"].ToString();
            vote.Type = Convert.ToInt32(reader["type"]) == 1 ? true : false;
            vote.StartTime = Convert.ToDateTime(reader["startTime"]).ToString();
            vote.EndTime = Convert.ToDateTime(reader["endTime"]).ToString();
            vote.CreateTime = Convert.ToDateTime(reader["createTime"]);
            return vote;
        }
    }
}
