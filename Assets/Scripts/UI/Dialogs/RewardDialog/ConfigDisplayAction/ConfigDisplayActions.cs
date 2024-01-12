using CryptoQuest.Gameplay.Loot;
using IndiGames.Core.Events;

namespace CryptoQuest.UI.Dialogs.RewardDialog.ConfigDisplayAction
{
    public class ConfigureMagicStoneLootDisplayAction : ActionBase
    {
        public UIRewardItem UIRewardItem;
        public MagicStoneLoot Loot;

        public ConfigureMagicStoneLootDisplayAction(MagicStoneLoot loot, UIRewardItem uiRewardItem)
        {
            UIRewardItem = uiRewardItem;
            Loot = loot;
        }
    }
}