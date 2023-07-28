using System;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    [Serializable]
    public abstract class AbstractButtonInfo
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        protected AbstractButtonInfo(string name, string value = "")
        {
            Name = name;
            Value = value;
        }

        public abstract void HandleClick();
    }

    [Serializable]
    public class ButtonInfo : AbstractButtonInfo
    {
        public ButtonInfo(IBattleUnit unit) : base(unit.UnitInfo.DisplayName) { }

        public override void HandleClick() { }
    }

    [Serializable]
    public class SkillAbstractButtonInfo : AbstractButtonInfo
    {
        private Action<IBattleUnit> _setTargetCallback;
        private IBattleUnit _unit;

        public SkillAbstractButtonInfo(IBattleUnit unit, Action<IBattleUnit> setTargetCallback)
            : base(unit.UnitInfo.DisplayName)
        {
            _unit = unit;
            _setTargetCallback = setTargetCallback;
        }

        public override void HandleClick()
        {
            _setTargetCallback.Invoke(_unit);
        }
    }
}