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

        protected override AbstractEffect CreateEffect()
        {
            return new CQInfiniteEffect(this);
        }
    }

    public class CQInfiniteEffect : InfiniteEffect
    {
        private CQInfiniteEffectScriptableObject _effectSO;

        public CQInfiniteEffect(CQInfiniteEffectScriptableObject effectSO)
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
            _unit = Owner.GetComponent<IBattleUnit>();
            foreach (EffectAttributeModifier modifier in EffectSO.EffectDetails.Modifiers)
            {
                var actionData = modifier.Value < 0 ? _effectSO.DecreaseActionDataSO : _effectSO.IncreaseActionDataSO;
                CharacterDataSO unitData = _unit.UnitData;
                if (unitData == null || actionData == null) return;

                actionData.Log.Clear();
                actionData.AddStringVar(UNIT_NAME_VARIABLE, unitData.DisplayName);
                actionData.AddStringVar(ATTRIBUTE_NAME_VARIABLE, modifier.AttributeSO.DisplayName);
                _effectSO.ActionEventSO.RaiseEvent(actionData);
            }
        }
    }
}