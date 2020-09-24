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
    public class PollMapperImpl:PollMapper
    {
        private readonly ILogger<PollMapperImpl> _logger;

        public PollMapperImpl(ILogger<PollMapperImpl> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根据Vid和Oid查询人数
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public int FindNumByVidAndOid(int vid, int oid)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT COUNT(1) FROM poll WHERE vid=@vid AND oid=@oid";
                    SqlParameter[] pms = {
                        new SqlParameter("@vid",vid),
                        new SqlParameter("@oid",oid)
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    row = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("PollMapperImpl类FindNumByVidAndOid()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 根据uid,vid查找对应poll
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public Poll FindPollByUidAndVid(Poll poll)
        {
            Poll p = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT uid,vid,oid FROM poll WHERE uid=@uid AND vid=@vid";
                    SqlParameter[] pms = {
                        new SqlParameter("@uid",poll.Uid),
                        new SqlParameter("@vid",poll.Vid),
                    };
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddRange(pms);
                    SqlDataReader reader = cmd.ExecuteReader();
                    p = new Poll();
                    p.Oids = new List<int>();
                    while (reader.Read())
                    {
                        p.Uid = Convert.ToInt32(reader["uid"]);
                        p.Vid = Convert.ToInt32(reader["vid"]);
                        p.Oids.Add(Convert.ToInt32(reader["oid"]));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("PollMapperImpl类FindPollByUidAndVid()方法异常：\n" + e.Message);
            }
            return p;
        }

        /// <summary>
        /// 保存投票记录
        /// </summary>
        /// <param name="poll"></param>
        /// <returns>返回影响行数</returns>
        public int SavePoll(Poll poll)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"INSERT INTO [poll](uid,vid,oid) values";
                    int len = poll.Oids.Count();
                    for (int i = 0; i < len; i++)
                    {
                        sql += "(" + poll.Uid + "," + poll.Vid + ",N'" + poll.Oids[i] + "')";
                        if (i != len - 1) sql += ",";
                    }
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    row = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("PollMapperImpl类SavePoll()方法异常：\n" + e.Message);
            }
            return row;
        }
    }
}
