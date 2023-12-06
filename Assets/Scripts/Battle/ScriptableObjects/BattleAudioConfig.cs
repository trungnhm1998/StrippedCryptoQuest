using CryptoQuest.Audio.AudioData;
using UnityEngine;

namespace CryptoQuest.Battle.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Audio Config")]
    public class BattleAudioConfig : ScriptableObject
    {
        [field: SerializeField] public AudioCueSO Intro { get; private set; }
        [field: SerializeField] public AudioCueSO Bgm { get; private set; }
    }
}