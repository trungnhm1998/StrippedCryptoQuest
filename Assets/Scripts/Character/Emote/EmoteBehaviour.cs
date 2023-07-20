using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class EmoteBehaviour : MonoBehaviour
    {
        [SerializeField] private EmoteSO _emote;
        [SerializeField] private SpriteRenderer _currentEmote;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _parameterAnimator;

        public void ShowEmote()
        {
            _currentEmote.sprite = _emote.ReactionIcon;
            _animator.ResetTrigger(_parameterAnimator);
            _animator.SetTrigger(_parameterAnimator);
        }
    }
}
