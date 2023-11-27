using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class RewardDialogData
    {
        public List<LootInfo> Loots { get; }

        public RewardDialogData(List<LootInfo> loots) => Loots = loots;

        public bool IsValid() => Loots is { Count: > 0 };
    }
}