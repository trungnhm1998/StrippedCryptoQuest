using CryptoQuest.GameHandler;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class BattleActionHandler : MonoBehaviour, IGameHandler
    {
        protected BattleActionHandler _nextHandler;

        public virtual void Handle(IBattleUnit unit)
        {
            if (_nextHandler == null) return;
            _nextHandler.Handle(unit);
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