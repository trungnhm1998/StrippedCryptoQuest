using System;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    public interface IEnemyCustomBehaviour
    {
        void OnEnable(EnemyBehaviour enemyBehaviour);
        void OnDisable();
    }

    [Serializable]
    public abstract class BaseBehaviour : IEnemyCustomBehaviour
    {
        protected EnemyBehaviour _enemyBehaviour;
        public virtual void OnEnable(EnemyBehaviour enemyBehaviour)
        {
            _enemyBehaviour = enemyBehaviour;
        }
        
        public abstract void OnDisable();
    }
}
