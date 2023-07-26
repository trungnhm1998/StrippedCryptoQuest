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

        public virtual void Handle(object request)
        {
            _nextHandler.Handle(request);
        }
    }
}