using UnityEngine;
using System.Collections.Generic;
using System;

namespace CryptoQuest.GameHandler
{
    public interface IGameHandler<T>
    {
        void Handle(T request);
        void Handle();
        IGameHandler<T> SetNext(IGameHandler<T> nextHandler);
    }

    public class EmptyGameHandler<T> : IGameHandler<T>
    {
        public void Handle(T request) { }
        public void Handle() { }

        public IGameHandler<T> SetNext(IGameHandler<T> nextHandler) => nextHandler;
    }
}