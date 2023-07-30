using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.Character.Emote
{
    public class ReactionController : MonoBehaviour
    {
        [SerializeField] private EmoteSO _emote;
        [SerializeField] private SpriteRenderer _emoteSpriteRenderer;
        [SerializeField] private Animator _animator;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            
            ShowEmote(_emote);
        }

        public void ShowEmote(EmoteSO emote)
        {
            _emoteSpriteRenderer.sprite = emote.ReactionIcon;
            _animator.Play(emote.ReactionStateName);
        }
    }
}