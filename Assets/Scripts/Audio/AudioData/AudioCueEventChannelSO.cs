using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/Audio Cue Event", fileName = "AudioCueEventChannelSO")]
    public class AudioCueEventChannelSO : ScriptableObject
    {
        public AudioCuePlayAction OnAudioCuePlayRequested;
        public AudioCueStopAction OnAudioCueStopRequested;
        public AudioCueFinishAction OnAudioCueFinishRequested;

        public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration)
        {
            if (OnAudioCuePlayRequested == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return AudioCueKey.Invalid;
            }

            Debug.Log($"Event was raised on {name} and someone was listening.");
            return OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration);
        }

        public bool RaiseStopEvent(AudioCueKey audioCueKey)
        {
            if (OnAudioCueStopRequested == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");

                return false;
            }

            return OnAudioCueStopRequested.Invoke(audioCueKey);
        }

        public bool RaiseFinishEvent(AudioCueKey audioCueKey)
        {
            if (OnAudioCueStopRequested != null)
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