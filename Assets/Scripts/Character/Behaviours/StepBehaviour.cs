using System;
using UnityEngine;

namespace CryptoQuest.Character.Behaviours
{
    public class StepBehaviour : MonoBehaviour
    {
        public event Action Step;

        [SerializeField] private float _distanceTilCountAsStep = .95f;

        private Vector3 _lastPosition;
        private float _distance;

        private void FixedUpdate()
        {
            OnCharacterStep();
        }

        /// <summary>
        /// find the magnitude of the vector (the distance) between the last position and the new position
        /// </summary>
        private void OnCharacterStep()
        {
            Vector2 position = transform.position;
            _distance +=
                Math.Abs(Vector2.Distance(_lastPosition, position)); // don't care about if moving backward or not
            _lastPosition = position;

            if (!IsDistanceTilNextStepReached()) return;
            _distance = 0;
            Step?.Invoke();
        }

        public void SetDistanceTilNextStep(float distance)
        {
            _distanceTilCountAsStep = distance;
        }

        private bool IsDistanceTilNextStepReached()
        {
            return _distance >= _distanceTilCountAsStep;
        }
    }
}
