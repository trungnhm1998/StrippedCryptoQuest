using UnityEngine;
using System.Collections.Generic;
using System;

namespace CryptoQuest.GameHandler
{
    public abstract class BaseGameHandler : MonoBehaviour, IGameHandler
    {
        private IGameHandler _nextHandler;

        public IGameHandler SetNext(IGameHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (_nextHandler == null) return null;
            return _nextHandler.Handle(request);
        }
    }
}