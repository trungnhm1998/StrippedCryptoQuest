using System;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    [Serializable]
    public abstract class AbstractButtonInfo
    {
        public string Name { get; protected set; }
        public string Value { get; protected set; }
        public bool IsInteractable { get; protected set; }

        protected AbstractButtonInfo(string name, string value = "", bool isInteractable = true)
        {
            Name = name;
            Value = value;
            IsInteractable = isInteractable;
        }

        public abstract void HandleClick();
    }

    [Serializable]
    public class EnemyGroupButtonInfo : AbstractButtonInfo
    {
        public EnemyGroupButtonInfo(CharacterDataSO unit, int numberOfEnemy) : base(unit.Name, "", false)
        {
            if (numberOfEnemy <= 1) return;
            Value = $"x{numberOfEnemy.ToString()}";
        }

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

    [Serializable]
    public class AbilityAbstractButtonInfo : AbstractButtonInfo
    {
        private AbilitySO _ability;
        private Action<AbilitySO> _setSkillCallback;

        public AbilityAbstractButtonInfo(Action<AbilitySO> setSkillCallback,
            AbilitySO ability)
            : base(ability.SkillInfo.SkillName.GetLocalizedString())

        {
            _setSkillCallback = setSkillCallback;
            _ability = ability;
        }

        public override void HandleClick()
        {
            _setSkillCallback.Invoke(_ability);
        }
    }

    [Serializable]
    public class ExpendableItemAbstractButtonInfo : AbstractButtonInfo
    {
        private UsableSO _item;
        private Action<UsableSO> _setItemCallback;

        public ExpendableItemAbstractButtonInfo(Action<UsableSO> setItemCallback,
            UsableSO item)
            : base(item.DisplayName.GetLocalizedString())
        {
            _setItemCallback = setItemCallback;
            _item = item;
        }

        public override void HandleClick()
        {
            _setItemCallback.Invoke(_item);
        }
    }
}