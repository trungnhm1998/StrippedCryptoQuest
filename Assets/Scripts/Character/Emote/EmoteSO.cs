using UnityEngine;
using UnityEngine.Search;

namespace CryptoQuest.Character.Emote
{
    [CreateAssetMenu(fileName = "Emote", menuName = "Crypto Quest/Emote")]
    public class EmoteSO : ScriptableObject
    {
        public Sprite ReactionIcon;
        [HideInInspector] public int ReactionStateName = Animator.StringToHash("Reaction");
    }
}