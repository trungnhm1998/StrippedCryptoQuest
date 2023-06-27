using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class TopDownController : ICharacterController2D
    {
        public float Speed { get; set; }
        public Vector2 InputVector { get; set; } = Vector2.zero;

        public Vector2 CalculateVelocity() => InputVector.normalized * Speed;

        public TopDownController(float speed)
        {
            Speed = speed;
        }
    }
}