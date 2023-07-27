using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/Audio Cue", fileName = "AudioCueSO")]
    public class AudioCueSO : ScriptableObject
    {
        public bool looping = false;
        [SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

        public AudioClip[] GetClips()
        {
            int numberOfClips = _audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];

            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = _audioClipGroups[i].GetNextClip();
            }

            return resultingClips;
        }
    }
}