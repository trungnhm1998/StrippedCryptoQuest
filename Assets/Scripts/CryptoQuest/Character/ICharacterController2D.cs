using UnityEngine;

namespace CryptoQuest.Character
{
    public interface ICharacterController2D
    {
        public float Speed { get; set; }
        public Vector2 InputVector { get; set; }
        public Vector2 CalculateVelocity();
    }
}