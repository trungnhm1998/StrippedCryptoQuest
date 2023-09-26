using System.Collections.Generic;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public interface ICommandDetailModel
    {
        List<ButtonInfoBase> Infos { get; }
        void AddInfo(params ButtonInfoBase[] infos);
    }
}