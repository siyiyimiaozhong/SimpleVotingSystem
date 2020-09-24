using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Mapper;
using VotingSystem.Models;

namespace VotingSystem.Service.ServiceImpl
{
    public class VoteServiceImpl : VoteService
    {
        private readonly VoteMapper _voteMapper;
        private readonly OptionMapper _optionMapper;
        private readonly PollMapper _pollMapper;

        public VoteServiceImpl(VoteMapper voteMapper, OptionMapper optionMapper, PollMapper pollMapper)
        {
            this._voteMapper = voteMapper;
            this._optionMapper = optionMapper;
            this._pollMapper = pollMapper;
        }

        /// <summary>
        /// 添加投票信息，并将选项写入对应表中
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        public int AddVote(VotingTheme vote)
        {
            int id = _voteMapper.SaveVoteTheme(vote);
            _optionMapper.SaveOptions(id, vote.Options);
            return id;
        }

        /// <summary>
        /// 根据id删除投票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteVote(int id)
        {
            return _voteMapper.DeleteVote(id);
        }

        /// <summary>
        /// 批量删除投票信息
        /// </summary>
        /// <param name="delStr"></param>
        /// <returns></returns>
        public int DeleteVotes(string delStr)
        {
            return _voteMapper.DeleteVotes(delStr);
        }

        /// <summary>
        /// 判断是否存在需要模糊查询的关键字，然后查询对应投票数据并根据投票id获取对应选项
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public PageObject<VotingTheme> FindAll(PageObject<VotingTheme> page)
        {
            if (page.Search == null || page.Search == "") page.Data = _voteMapper.FindAll(page);
            else page.Data = _voteMapper.FindAllBySearch(page);

            if (page.Search == null || page.Search == "") page.TotalCount = _voteMapper.FindNum();
            else page.TotalCount = _voteMapper.FindNumBySearch(page);

            int len = page.Data.Count();
            for(int i=0;i<len;i++)
            {
                page.Data[i].Options = _optionMapper.FindOptionByVid(page.Data[i].Id);
            }
            return page;
        }

        /// <summary>
        /// 获取正在进行的投票任务数
        /// </summary>
        /// <returns></returns>
        public int GetTackingNum()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            return _voteMapper.GetTackingNum(time);
        }

        /// <summary>
        /// 获取总的任务数
        /// </summary>
        /// <returns></returns>
        public int GetVoteNum()
        {
            return _voteMapper.FindNum();
        }

        /// <summary>
        /// 修改投票信息，1.修改投票信息，删除所有选项，重新添加选项
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        public int UpdateVote(VotingTheme vote)
        {
            int row = _voteMapper.UpdateVote(vote);
            if (row == 0) return 0;
            row = _optionMapper.DeleteOptionsByVid(vote.Id);
            if (row == 0) return 0;
            row = _optionMapper.SaveOptions(vote.Id, vote.Options);
            return row;
        }

        public VotingTheme VoteDetail(VotingTheme vote)
        {
            int len = vote.Options.Count();
            vote.Count = 0;
            for(int i=0;i<len;i++)
            {
                vote.Options[i].Num = _pollMapper.FindNumByVidAndOid(vote.Id, vote.Options[i].Id);
                vote.Count += vote.Options[i].Num;
            }
            return vote;
        }
    }
}
