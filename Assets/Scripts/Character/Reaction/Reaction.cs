using UnityEngine;

namespace CryptoQuest.Character.Reaction
{
    [CreateAssetMenu(fileName = "Reaction", menuName = "Crypto Quest/Character/Reaction")]
    public class Reaction : ScriptableObject
    {
        public Sprite ReactionIcon;
        [HideInInspector] public int ReactionStateName = Animator.StringToHash("Reaction");
    }
}