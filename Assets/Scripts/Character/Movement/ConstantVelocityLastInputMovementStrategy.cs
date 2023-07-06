using System;
using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.Movement
{
    public class ConstantVelocityLastInputMovementStrategy : IPlayerVelocityStrategy
    {
        private readonly CharacterBehaviour _characterBehaviour;

        public ConstantVelocityLastInputMovementStrategy(CharacterBehaviour characterBehaviour)
        {
            _characterBehaviour = characterBehaviour;
        }

        public Vector2 CalculateVelocity(Vector2 inputVector, float speed)
        {
            _characterBehaviour.SetFacingDirection(inputVector);

            return inputVector * speed;
        }

    }
}