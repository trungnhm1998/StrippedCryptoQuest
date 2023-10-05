using System.Collections.Generic;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public interface ICommandDetailModel
    {
        List<ButtonInfoBase> Infos { get; }
        void AddInfo(params ButtonInfoBase[] infos);
    }

    public class CommandDetailModel : ICommandDetailModel
    {
        private List<ButtonInfoBase> _infos;
        public List<ButtonInfoBase> Infos => _infos;

        public CommandDetailModel(params ButtonInfoBase[] infos)
        {
            _infos = new List<ButtonInfoBase>(infos);
        }

        public void AddInfo(params ButtonInfoBase[] infos)
        {
            _infos.AddRange(infos);
        }
    }
}