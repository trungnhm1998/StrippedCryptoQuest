using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public interface IPlayerVelocityStrategy
    {
        public Vector2 CalculateVelocity(Vector2 inputVector, float speed);
    }
}