using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/BGM Cue", fileName = "BGMCueSO")]
    public class BGMCueSO : AudioCueSO 
    {
        public BGMCueSO()
        {
            Looping = true;
        }
    }
}