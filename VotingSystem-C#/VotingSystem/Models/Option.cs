namespace VotingSystem.Models
{
    /// <summary>
    /// 选项类
    /// </summary>
    public class Option
    {
        public int Id { get; set; }
        public int Choice { get; set; }
        public string Msg { get; set; }
        public int Num { get; set; }
    }
}