using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingSystem.Utils
{
    /// <summary>
    /// 响应对象
    /// </summary>
    public class JSONResult
    {
        public int Status { get; set; } //响应业务状态
        public string Msg { get; set; } //响应消息
        public object Data { get; set; } //响应数据

        public JSONResult()
        {

        }

        public JSONResult(int status, String msg, Object data)
        {
            this.Status = status;
            this.Msg = msg;
            this.Data = data;
        }

        public JSONResult(Object data)
        {
            this.Status = 200;
            this.Msg = "OK";
            this.Data = data;
        }

        public static JSONResult Build(int status, String msg, Object data)
        {
            return new JSONResult(status, msg, data);
        }

        public static JSONResult Ok(Object data)
        {
            return new JSONResult(data);
        }

        public static JSONResult Ok()
        {
            return new JSONResult(null);
        }

        public static JSONResult ErrorMsg(String msg)
        {
            return new JSONResult(500, msg, null);
        }

        public static JSONResult ErrorMap(Object data)
        {
            return new JSONResult(501, "error", data);
        }

        public static JSONResult ErrorTokenMsg(String msg)
        {
            return new JSONResult(502, msg, null);
        }

        public static JSONResult ErrorException(String msg)
        {
            return new JSONResult(555, msg, null);
        }
    }
}
