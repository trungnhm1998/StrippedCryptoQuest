using CQAttribute = CryptoQuest.Character.Attributes.AttributeScriptableObject;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;
using UnityEngine.Localization.Settings;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Effects
{
    [CreateAssetMenu(fileName = "CQInfiniteEffect", menuName = "Indigames Ability System/Effects/CQ Infinite Effect")]
    public class CQInfiniteEffectScriptableObject : EffectScriptableObject
    {
        public BattleActionDataEventChannelSO ActionEventSO;
        public BattleActionDataSO IncreaseActionDataSO;
        public BattleActionDataSO DecreaseActionDataSO;

        protected override GameplayEffectSpec CreateEffect()
        {
            return new CqInfiniteEffectSpec(this);
        }
    }

    public class CqInfiniteEffectSpec : InfiniteEffectSpec
    {
        private CQInfiniteEffectScriptableObject _effectSO;

        public CqInfiniteEffectSpec(CQInfiniteEffectScriptableObject effectSO)
        {
            _effectSO = effectSO;
        }

        protected const string UNIT_NAME_VARIABLE = "unitName";
        protected const string ATTRIBUTE_NAME_VARIABLE = "attributeName";

        protected IBattleUnit _unit;

        public override void Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);
            LogEffect();
        }

        private void LogEffect()
        {
            _unit = Source.GetComponent<IBattleUnit>();
            foreach (EffectAttributeModifier modifier in Def.EffectDetails.Modifiers)
            {
                var actionData = modifier.Value < 0 ? _effectSO.DecreaseActionDataSO : _effectSO.IncreaseActionDataSO;
                if (actionData == null) return;

                actionData.Log.Clear();
                actionData.AddStringVar(UNIT_NAME_VARIABLE, _unit.UnitInfo.DisplayName);
                actionData.AddStringVar(ATTRIBUTE_NAME_VARIABLE, ((CQAttribute)modifier.Attribute).DisplayName);
                _effectSO.ActionEventSO.RaiseEvent(actionData);
            }
        }
    }
}