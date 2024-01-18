using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    /// <summary>
    /// Behaviour of enemy that attach in enemy prefab to custom enemy behaviour
    /// for each enemy without changing any thing in enemy data. 
    /// </summary>
    public class EnemyCustomBehaviour : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private IEnemyCustomBehaviour[] _behaviours;
        public EnemyBehaviour _enemyBehaviour { get; private set;}

        private void OnEnable()
        {
            _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
            if (_enemyBehaviour == null)
            {
                Debug.LogWarning($"This custom behaviour dont attach to any enemy");
            }
            
            foreach (var behaviour in _behaviours)
            {
                behaviour.OnEnable(_enemyBehaviour);
            }
        }

        private void OnDisable()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.OnDisable();
            }
        }
    }
}