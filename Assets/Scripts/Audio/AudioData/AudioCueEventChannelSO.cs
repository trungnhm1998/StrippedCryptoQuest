using CryptoQuest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Audio.AudioData
{
    public class AudioCueEventChannelSO : ScriptableObject
    {
        public event UnityAction<AudioCueSO> AudioPlayRequested;
        public event UnityAction<AudioCueSO> AudioStopRequested;

        /// <summary>
        /// Raises the play event for an audio cue.
        /// </summary>
        /// <param name="cueSO">The AudioCueSO representing the audio cue to play.</param>
        /// <returns>The AudioCueKey associated with the played audio cue.</returns>
        public void PlayAudio(AudioCueSO cueSO) => AudioPlayRequested.SafeInvoke(cueSO);

        /// <summary>
        /// Raises the stop event for the audio cue associated with the provided AudioCueKey.
        /// </summary>
        /// <param name="cueSO">The AudioCueKey representing the audio cue to stop.</param>
        /// <returns>True if the stop event was handled successfully; otherwise, false.</returns>
        public void StopAudio(AudioCueSO cueSO) => AudioStopRequested.SafeInvoke(cueSO);
    }
}