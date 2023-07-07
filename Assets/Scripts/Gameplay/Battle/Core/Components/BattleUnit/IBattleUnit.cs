using System.Collections;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public interface IBattleUnit
    {
        public GameObject gameObject { get; } 
        public List<string> ExecuteLogs { get; }
        public AbilitySystemBehaviour Owner { get; }
        public BattleTeam OpponentTeam { get; }
        public string OriginalName { get; }
        
        public void Init(BattleTeam team, AbilitySystemBehaviour owner);
        public void SetOpponentTeams(BattleTeam opponentTeam);
        public IEnumerator Prepare();
        public IEnumerator Execute();
        public IEnumerator Resolve();
        public void OnDeath();
        public void SelectSkill(AbstractAbility selectedSkill);
        public void SelectSingleTarget(AbilitySystemBehaviour target);
    }
}
    