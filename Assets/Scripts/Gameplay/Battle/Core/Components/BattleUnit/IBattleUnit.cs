using System.Collections;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public interface IBattleUnit
    {
        void Init(BattleTeam team, AbilitySystemBehaviour owner);
        void SetOpponentTeams(BattleTeam opponentTeam);
        IEnumerator Prepare();
        IEnumerator Execute();
        IEnumerator Resolve();
        void OnDeath();
        AbilitySystemBehaviour GetOwner();
        BattleTeam GetOpponent();
        void SelectSkill(AbstractAbility selectedSkill);
        void SelectSingleTarget(AbilitySystemBehaviour target);
        string GetOriginalName();
    }
}
    