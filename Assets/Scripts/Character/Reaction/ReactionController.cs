using UnityEngine;

namespace CryptoQuest.Character.Reaction
{
    public class ReactionController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _emoteSpriteRenderer;
        [SerializeField] private Animator _animator;

        public void ShowReaction(Reaction emote)
        {
            _emoteSpriteRenderer.sprite = emote.ReactionIcon;
            _animator.Play(emote.ReactionStateName);
        }
    }
}