using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingSystem.Models
{
    /// <summary>
    /// 投票属性类
    /// </summary>
    public class VotingTheme
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Type { get; set; } // 是否是多选
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public List<Option>  Options { get; set; }
        public int Count { get; set; }
    }
}
