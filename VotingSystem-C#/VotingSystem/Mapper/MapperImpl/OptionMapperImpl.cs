using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using VotingSystem.conf;
using VotingSystem.Models;

namespace VotingSystem.Mapper.MapperImpl
{
    public class OptionMapperImpl : OptionMapper
    {
        private readonly ILogger<OptionMapperImpl> _logger;

        public OptionMapperImpl(ILogger<OptionMapperImpl> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根据投票Id删除所有对应选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteOptionsByVid(int id)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"DELETE FROM [option] WHERE vid=@vid";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("@vid", id));
                    row = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("OptionMapperImpl类DeleteOptionsByVid()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 根据vid获取所有选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Option> FindOptionByVid(int id)
        {
            List<Option> options = new List<Option>();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"SELECT id,choice,msg FROM [option] WHERE vid=@id";
                    connection.Open();
                    
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Option option = new Option();
                        option = GetOption(option, reader);
                        options.Add(option);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("OptionMapperImpl类FindOptionByVid()方法异常：\n" + e.Message);
            }
            return options;
        }

        public int SaveOptions(int id, List<Option> options)
        {
            int row = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    string sql = @"INSERT INTO [option](vid,choice,msg) values";
                    int len = options.Count();
                    for(int i=0;i<len;i++)
                    {
                        sql += "(" + id + "," + options[i].Choice + ",N'" + options[i].Msg + "')";
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
                _logger.LogInformation("OptionMapperImpl类SaveOptions()方法异常：\n" + e.Message);
            }
            return row;
        }

        /// <summary>
        /// 通过reader封装option
        /// </summary>
        /// <param name="option"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Option GetOption(Option option, SqlDataReader reader)
        {
            option.Id = Convert.ToInt32(reader["id"]);
            option.Choice = Convert.ToInt32(reader["choice"]);
            option.Msg = reader["msg"].ToString();
            return option;
        }
    }
}
