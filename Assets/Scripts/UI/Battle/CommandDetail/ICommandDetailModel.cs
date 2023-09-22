using System.Collections.Generic;

namespace CryptoQuest.UI.Battle.CommandDetail
{
    public interface ICommandDetailModel
    {
        List<ButtonInfoBase> Infos { get; }
        void AddInfo(params ButtonInfoBase[] infos);
    }
}