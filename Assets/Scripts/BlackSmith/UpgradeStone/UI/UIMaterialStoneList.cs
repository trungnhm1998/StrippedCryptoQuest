using System;
using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIMaterialStoneList : UIUpgradableStoneList
    {
        public event Action<UIUpgradableStone> MaterialSelected;

        protected override void OnSelectItem(UIUpgradableStone ui)
        {
            MaterialSelected?.Invoke(ui);
        }
    }
}