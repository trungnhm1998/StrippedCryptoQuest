using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using ILogger = CryptoQuest.Gameplay.Battle.Core.Components.Logger.ILogger;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public interface IBattleUnit
    {
        ILogger Logger { get; }
        AbilitySystemBehaviour Owner { get; }
        BattleTeam OpponentTeam { get; }
        bool IsDead { get; }
        AbstractAbility NormalAttack { get; }
        
        CharacterDataSO UnitData { get; set; }
        void Init(BattleTeam team, AbilitySystemBehaviour owner);
        void SetOpponentTeams(BattleTeam opponentTeam);
        IEnumerator Prepare();
        IEnumerator Execute();
        IEnumerator Resolve();
        void OnDeath();
        void SelectSkill(AbstractAbility selectedSkill);
        void SelectSingleTarget(AbilitySystemBehaviour target);
    }
}
    