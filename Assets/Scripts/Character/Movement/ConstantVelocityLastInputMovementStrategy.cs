using System;
using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantVelocityLastInputMovementStrategy : IPlayerVelocityStrategy
    {
        public Vector2 CalculateVelocity(Vector2 inputVector, float speed) => inputVector * speed;
    }
}