using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Effects
{
    [CreateAssetMenu(fileName = "CQInfiniteEffect", menuName = "Indigames Ability System/Effects/CQ Infinite Effect")]
    public class CQInfiniteEffectScriptableObject : EffectScriptableObject<CQInfiniteEffect> {}

    public class CQInfiniteEffect : InfiniteEffect
    {
        protected const string BATTLE_PROMT_TABLE = "BattlePromt";
        protected const string ATTRIBUTE_INCREASED_KEY = "ATTRIBUTE_INCREASED";
        protected const string ATTRIBUTE_DECREASED_KEY = "ATTRIBUTE_DECREASED";
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
                string promtKey = modifier.Value < 0 ? ATTRIBUTE_DECREASED_KEY : ATTRIBUTE_INCREASED_KEY;
                string promt = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, promtKey);
                CharacterDataSO unitData = _unit.UnitData;
                if (unitData == null) return;
                _unit.Logger.Log(string.Format(promt, unitData.DisplayName, modifier.AttributeSO.DisplayName));
            }

        }
    }
}