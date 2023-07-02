using UnityEngine;

namespace CryptoQuest.Camera
{
    public class CameraManagerSO : ScriptableObject {
        private Vector2 _playerPosition;
        public Vector2 PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
    }
}