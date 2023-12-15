using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class UpgradableStoneDataMapping : ScriptableObject
    {
        [field: SerializeField] public List<UpgradableStoneData> Datas { get; private set; }
    }
}