using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.Emote
{
    public class EmoteBehaviour : MonoBehaviour
    {
        [SerializeField] private EmoteSO _emote;
        [SerializeField] private SpriteRenderer _currentEmote;
        [SerializeField] private Animator _animator;
        private static readonly int AnimIsShowingEmote = Animator.StringToHash("IsShowingEmote");

        public void ShowEmote()
        {
            _currentEmote.sprite = _emote.ReactionIcon;
            _animator.ResetTrigger(AnimIsShowingEmote);
            _animator.SetTrigger(AnimIsShowingEmote);
        }
    }
}
