using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/Audio Cue Event", fileName = "AudioCueEventChannelSO")]
    public class AudioCueEventChannelSO : ScriptableObject
    {
        public AudioCuePlayAction OnAudioCuePlayRequested;
        public AudioCueStopAction OnAudioCueStopRequested;
        public AudioCueFinishAction OnAudioCueFinishRequested;

#if UNITY_EDITOR
        [SerializeField] private AudioCueSO _audioCue = default;
        public AudioCueSO AudioCue => _audioCue;
#endif
        /// <summary>
        /// Raises the play event for an audio cue with the given audio configuration.
        /// </summary>
        /// <param name="audioCue">The AudioCueSO representing the audio cue to play.</param>
        /// <param name="audioConfiguration">The AudioConfigurationSO for audio playback settings.</param>
        /// <returns>The AudioCueKey associated with the played audio cue.</returns>
        public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration)
        {
            if (OnAudioCuePlayRequested == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return AudioCueKey.Invalid;
            }

#if UNITY_EDITOR
            _audioCue = audioCue;
#endif
            return OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration);
        }

        /// <summary>
        /// Raises the stop event for the audio cue associated with the provided AudioCueKey.
        /// </summary>
        /// <param name="audioCueKey">The AudioCueKey representing the audio cue to stop.</param>
        /// <returns>True if the stop event was handled successfully; otherwise, false.</returns>
        public bool RaiseStopEvent(AudioCueKey audioCueKey)
        {
            if (OnAudioCueStopRequested == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return false;
            }

            return OnAudioCueStopRequested.Invoke(audioCueKey);
        }

        /// <summary>
        /// Raises the finish event for the audio cue associated with the provided AudioCueKey.
        /// </summary>
        /// <param name="audioCueKey">The AudioCueKey representing the audio cue that finished playing.</param>
        /// <returns>True if the finish event was handled successfully; otherwise, false.</returns>
        public bool RaiseFinishEvent(AudioCueKey audioCueKey)
        {
            if (OnAudioCueFinishRequested == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return false;
            }

            return OnAudioCueFinishRequested.Invoke(audioCueKey);
        }
    }

    public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration);

    public delegate bool AudioCueStopAction(AudioCueKey emitterKey);

    public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);
}