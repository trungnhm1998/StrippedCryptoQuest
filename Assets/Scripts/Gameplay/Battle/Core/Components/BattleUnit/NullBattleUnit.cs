using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using ILogger = CryptoQuest.Gameplay.Battle.Core.Components.Logger.ILogger;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class NullBattleUnit : IBattleUnit
    {
        private static NullBattleUnit _instance;
        public static IBattleUnit Instance => _instance ??= new NullBattleUnit();

        public AbilitySystemBehaviour Owner { get; }
        public BattleTeam OpponentTeam { get; }
        
        public bool IsDead { get; }
        public AbstractAbility NormalAttack { get; }
        
        public CharacterDataSO UnitData { get; set; }
        public CharacterInformation UnitInfo { get; private set; }

        public void Init(BattleTeam team, AbilitySystemBehaviour owner) {}
        public void SetOpponentTeams(BattleTeam opponentTeam) {}

        public IEnumerator Prepare()
        {
            yield return null;
        }

        public IEnumerator Execute()
        {
            yield return null;
        }

        public IEnumerator Resolve()
        {
            yield return null;
        }

        public void OnDeath() {}
        public void SelectSkill(AbstractAbility selectedSkill) {}
        public void SelectSingleTarget(AbilitySystemBehaviour target) {}
    }
}
    