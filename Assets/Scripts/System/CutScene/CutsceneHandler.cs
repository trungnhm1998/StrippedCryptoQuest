using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.System.Cutscene
{
    public class CutsceneHandler : MonoBehaviour
    {
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private Transform _heroEndPosition;

        public void HandleDirectorStopped()
        {
            _gameplayBus.Hero.transform.position = _heroEndPosition.position;
        }
    }
}