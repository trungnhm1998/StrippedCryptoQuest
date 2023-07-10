using System.Collections;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public interface IBattleUnit
    {
        List<string> ExecuteLogs { get; }
        AbilitySystemBehaviour Owner { get; }
        BattleTeam OpponentTeam { get; }
        string OriginalName { get; }
        bool IsDead { get; }
        
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
    