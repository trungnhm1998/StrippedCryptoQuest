using System;
using CryptoQuest.GameHandler;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class BattleActionHandler : MonoBehaviour, IGameHandler
    {
        public BattleInfo CurrentBattleInfo;
        private BattleActionHandler _nextHandler;

        public virtual void Handle(IBattleUnit unit)
        {
            _nextHandler.Handle(unit);
        }

        public virtual IGameHandler SetNext(BattleActionHandler handler)
        {
            _nextHandler = handler;
            return _nextHandler;
        }

        public IGameHandler SetNext(IGameHandler handler) => handler;
        public virtual void Handle(object request) { }
    }
}