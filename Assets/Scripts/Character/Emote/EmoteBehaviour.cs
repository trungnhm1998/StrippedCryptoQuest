using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class EmoteBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EmoteSO> _listEmote;
        [SerializeField] private SpriteRenderer _currentEmote;
        [SerializeField] private Animator _animator;
        
        public void SetReactionIcon(TypeOfEmote type)
        {
            foreach (EmoteSO emote in _listEmote)
            {
                if (emote.TypeOfEmote == type)
                {
                    _currentEmote.sprite = emote.ReactionIcon;
                    _animator.SetTrigger("ReactionEmote");
                }
            }
        }
    }
}
