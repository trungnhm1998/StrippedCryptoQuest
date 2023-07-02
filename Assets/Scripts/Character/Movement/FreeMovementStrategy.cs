using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class FreeMovementStrategy : IPlayerVelocityStrategy
    {
        public Vector2 CalculateVelocity(Vector2 inputVector, float speed) => inputVector.normalized * speed;
    }
}