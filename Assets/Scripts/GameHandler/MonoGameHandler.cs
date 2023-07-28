using UnityEngine;

namespace CryptoQuest.GameHandler
{
    public class MonoGameHandler<T> : MonoBehaviour, IGameHandler<T>
    {
        private IGameHandler<T> _nextHandler;
        public IGameHandler<T> NextHandler => _nextHandler;
        
        public IGameHandler<T> SetNext(IGameHandler<T> handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(T request)
        {
            NextHandler?.Handle();
        }

        public virtual void Handle() 
        {
            NextHandler?.Handle();
        }
    }
}