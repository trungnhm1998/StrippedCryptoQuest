using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Gameplay.Battle
{
    /// <summary>
    /// Skill will end right after activate like normal attack
    /// </summary>
    [CreateAssetMenu(fileName = "CQSkillSO", menuName = "Gameplay/Battle/Abilities/Crypto Quest Skill")]
    public class CQSkillSO : EffectAbilitySO
    {
        public string SkillName = "";
        public string PromtKey = "";
        protected override AbstractAbility CreateAbility()
        {
            var skill = new CQSkill();
            return skill;
        }
    }

    public class CQSkill : EffectAbility
    {
        protected const string BATTLE_PROMT_TABLE = "BattlePromt";
        protected IBattleUnit _unit;
        protected new CQSkillSO AbilitySO => (CQSkillSO) _abilitySO;
        
        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            base.OnAbilityGranted(skillSpec);
            _unit = Owner.GetComponent<IBattleUnit>();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            if (_unit == null) yield break;
            SkillActivatePromt();
            yield return base.InternalActiveAbility();
        }

        protected virtual void SkillActivatePromt()
        {
            if (AbilitySO.PromtKey == "") return;
            string normalAttackText = LocalizationSettings.StringDatabase.GetLocalizedString(BATTLE_PROMT_TABLE, AbilitySO.PromtKey);
            _unit.ExecuteLogs.Add(string.Format(normalAttackText, Owner.gameObject.name));
        }
    }
}