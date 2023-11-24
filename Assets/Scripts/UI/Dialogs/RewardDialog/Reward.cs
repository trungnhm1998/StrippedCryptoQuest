using CryptoQuest.Gameplay.Loot;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public abstract class Reward
    {
        private readonly bool _isTop;

        protected Reward(bool isTop = false) => _isTop = isTop;

        private UIRewardItem _ui;

        public void CreateUI(UIRewardDialog ctx)
        {
            _ui = ctx.InstantiateReward(_isTop ? ctx.TopContainer : ctx.BottomContainer);
            OnCreateUI(_ui);
        }

        protected abstract void OnCreateUI(UIRewardItem ui);
    }

    public class AmountReward : Reward
    {
        private readonly float _amount;
        private readonly LocalizedString _localizedString;

        public AmountReward(float currencyLootInfo, LocalizedString localizedString) : base(true)
        {
            _amount = currencyLootInfo;
            _localizedString = localizedString;
        }

        protected override void OnCreateUI(UIRewardItem ui)
        {
            ui.RewardContent.StringReference = _localizedString;
            ui.RewardContent.StringReference.StringChanged += s =>
            {
                var text = $"{_amount} {s}";
                ui.SetText(text);
            };
        }
    }

    public class GenericLocalizedReward : Reward
    {
        private readonly LocalizedString _value;

        public GenericLocalizedReward(LocalizedString value)
        {
            _value = value;
        }

        protected override void OnCreateUI(UIRewardItem ui)
        {
            ui.RewardContent.StringReference = _value;
        }
    }

    public class ConsumableReward : Reward
    {
        private readonly ConsumableLootInfo _consumableLootInfo;

        public ConsumableReward(ConsumableLootInfo consumableLootInfo)
        {
            _consumableLootInfo = consumableLootInfo;
        }

        protected override void OnCreateUI(UIRewardItem ui)
        {
            ui.RewardContent.StringReference = _consumableLootInfo.Item.Data.DisplayName;
            ui.RewardContent.StringReference.StringChanged += s =>
            {
                var quantityText = $" X{_consumableLootInfo.Item.Quantity}";
                var text = $"{s}{quantityText}";
                ui.SetText(text);
            };
        }
    }

    public class GenericReward : Reward
    {
        private readonly string _txt;
        public GenericReward(string str) : base(true) => _txt = str;
        protected override void OnCreateUI(UIRewardItem ui) => ui.SetText(_txt);
    }
}