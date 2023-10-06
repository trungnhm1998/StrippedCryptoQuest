using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class RewardDialogData
    {
        public readonly List<Reward> RewardsInfos;

        public RewardDialogData(List<LootInfo> loots)
        {
            RewardsInfos = new();
            for (int i = 0; i < loots.Count; i++)
            {
                var loot = loots[i];
                RewardsInfos.Add(loot.CreateRewardUI());
            }
        }

        public bool IsValid() => RewardsInfos != null && RewardsInfos.Count > 0;
    }
}