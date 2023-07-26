using UnityEngine;
using System.Collections.Generic;
using System;

namespace CryptoQuest.GameHandler
{
    public abstract class BaseGameHandler<T> : MonoBehaviour, IGameHandler<T>
    {
        private IGameHandler<T> _nextHandler = new EmptyGameHandler<T>();

        public IGameHandler<T> SetNext(IGameHandler<T> handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(T request)
        {
            _nextHandler.Handle(request);
        }

        public virtual void Handle()
        {
            _nextHandler.Handle();
        }
    }
}