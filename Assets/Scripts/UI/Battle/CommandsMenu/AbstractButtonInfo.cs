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

        public abstract void OnHandleClick();
    }

    [Serializable]
    public class EnemyGroupButtonInfo : AbstractButtonInfo
    {
        private Action<int> _setCallback;
        private int _index;

        public EnemyGroupButtonInfo(CharacterDataSO unit, int numberOfEnemy, bool isInteractable = false, int index = 0,
            Action<int> callback = null)
            : base(unit.Name, "", isInteractable)
        {
            if (numberOfEnemy < 1) return;
            Value = $"x{numberOfEnemy.ToString()}";
            _setCallback = callback;
            _index = index;
        }

        public override void OnHandleClick()
        {
            _setCallback?.Invoke(_index);
        }
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

        public override void OnHandleClick()
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

        public override void OnHandleClick()
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

        public override void OnHandleClick()
        {
            _setItemCallback.Invoke(_item);
        }
    }
    // [Serializable]
}