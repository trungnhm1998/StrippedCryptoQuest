using Cinemachine;
using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] GameplayBus _gameplayBus;
        [SerializeField] CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private bool _isStaticCamera;

        private void OnEnable() {
            _gameplayBus.HeroSpawned += HeroSpawned;
        }

        private void OnDisable() {
            _gameplayBus.HeroSpawned -= HeroSpawned;
            
        }

        private void HeroSpawned()
        {
            if (_isStaticCamera) return;
            var hero = _gameplayBus.Hero;
            _virtualCamera.Follow = hero.transform;
        }
    }
}