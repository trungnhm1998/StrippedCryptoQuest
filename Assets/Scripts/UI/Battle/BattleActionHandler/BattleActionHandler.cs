using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.GameHandler;

namespace CryptoQuest.UI.Battle
{
    public class BattleActionHandler : MonoBehaviour, IGameHandler
    {
        protected BattleActionHandler _nextHandler;

        public virtual object Handle(IBattleUnit unit)
        {
            if (_nextHandler == null) return null;
            return _nextHandler.Handle(unit);
        }
        
        public virtual IGameHandler SetNext(BattleActionHandler handler)
        {
            _nextHandler = handler;
            return _nextHandler;
        }

        public IGameHandler SetNext(IGameHandler handler) => handler;
        public virtual object Handle(object request) => request;

    }
}