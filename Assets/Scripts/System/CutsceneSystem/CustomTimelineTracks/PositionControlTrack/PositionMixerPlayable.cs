using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Timeline.Position
{
    /// <summary>
    /// This will cause the game object transform to be set to the position specified by the PositionClip.
    /// </summary>
    [Serializable]
    public class PositionMixerPlayable : PlayableBehaviour
    {
        private Vector3 _initialPosition;

        private Transform _boundTransform;


        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData);

            if (_boundTransform == null) return;

            var inputCount = playable.GetInputCount();


            for (var i = 0; i < inputCount; i++)
            {
                if (!(playable.GetInputWeight(i) > 0)) continue;

                var input = (ScriptPlayable<PositionPlayableBehaviour>)playable.GetInput(i);
                _boundTransform.localPosition = input.GetBehaviour().Position;
                break;
            }
        }

        private void SetDefaults(object playerData)
        {
            if (playerData is GameObject gameObject)
            {
                // support run time binding of game object
                playerData = gameObject.transform;
            }

            var transform = (Transform)playerData;

            if (transform == _boundTransform) return;

            _boundTransform = transform;
            if (_boundTransform != null)
            {
                _initialPosition = _boundTransform.localPosition;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (_boundTransform == null) return;

            // Reset the position to the initial position when the timeline is paused only for editor mode.
            if (!Application.isPlaying)
            {
                _boundTransform.localPosition = _initialPosition;
            }
        }
    }
}