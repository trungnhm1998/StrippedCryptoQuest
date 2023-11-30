using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using System.Linq;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Map.CheckPoint
{
    public class AutoCheckPoint : MonoBehaviour
    {
        [SerializeField]
        private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private int _lookDirection;

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += SaveCheckPoint;

            var colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders.ToArray())
            {
                Destroy(collider);
            }    
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= SaveCheckPoint;
        }

        private void SaveCheckPoint()
        {
            var checkPointController = ServiceProvider.GetService<ICheckPointController>();
            checkPointController.SaveCheckPoint(transform.position, _lookDirection);
        }

        /// <summary>
        /// This will be called when SuperTiled2Unity has finished importing the component.
        /// 0 : Left - 1 : Right - 2 : Up - 3: Down
        /// </summary>
        /// <param name="Direction"></param>
        public void Direction(int direction)
        {
            _lookDirection = direction;
        }
    }
}
