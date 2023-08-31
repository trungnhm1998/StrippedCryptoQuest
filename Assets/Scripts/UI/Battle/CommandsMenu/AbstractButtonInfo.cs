using System;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem;

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
        private SimpleGameplayAbilitySpec _ability;
        private Action<SimpleGameplayAbilitySpec> _setSkillCallback;

        public AbilityAbstractButtonInfo(Action<SimpleGameplayAbilitySpec> setSkillCallback,
            SimpleGameplayAbilitySpec ability)
            : base(GetSkillName(ability))
        {
            _setSkillCallback = setSkillCallback;
            _ability = ability;
            // TODO: REFACTOR BATTLE
            // Value = $"{_ability.AbilityDef.Info.Cost.ToString()}";
        }

        public override void OnHandleClick()
        {
            _setSkillCallback.Invoke(_ability);
        }

        public static string GetSkillName(SimpleGameplayAbilitySpec ability)
        {
            // return ability.AbilityDef.Info.SkillName.GetLocalizedString();
            // TODO: refactor battle
            return "MOCK";
        }
    }

    [Serializable]
    public class ExpendableItemAbstractButtonInfo : AbstractButtonInfo
    {
        private UsableSO _item;
        private Action<UsableSO> _setItemCallback;

        public ExpendableItemAbstractButtonInfo(Action<UsableSO> setItemCallback,
            UsableInfo itemInfo)
            : base(itemInfo.Data.DisplayName.GetLocalizedString())
        {
            _setItemCallback = setItemCallback;
            _item = itemInfo.Data;
            Value = $"x{itemInfo.Quantity.ToString()}";
        }

        public override void OnHandleClick()
        {
            _setItemCallback.Invoke(_item);
        }
    }
    // [Serializable]
}