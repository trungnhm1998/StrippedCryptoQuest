using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class CharacterUnit : BattleUnitBase
    {
        public override void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            base.Init(team, owner);
            UnitInfo.Owner = owner;
            GrantDefaulSkills();
        }

        private void GrantDefaulSkills()
        {
            NormalAttack = Owner.GiveAbility(UnitData.NormalAttack);
            foreach (var skill in UnitData.GrantedSkills)
            {
                Owner.GiveAbility(skill);
            }
        }
    }
}