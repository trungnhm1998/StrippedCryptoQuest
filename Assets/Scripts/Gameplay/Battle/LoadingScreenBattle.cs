using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    public class LoadingScreenBattle : MonoBehaviour
    {
        public enum ScreenTransition
        {
            Battle = 0,
            Ocarina = 1
        }
        [SerializeField] private Animator _animator;
        [SerializeField] private ScreenTransition _loadScreen;
        [SerializeField] private List<SpriteRenderer> _listSprite;
        private bool _isLoadScreen;

        public virtual bool IsTransitionScreen
        {
            set => _animator.SetBool(AnimIsScreenTransition, value);
        }
        private static readonly int AnimIsScreenTransition = Animator.StringToHash("Transition");

        public void LoadTransitionScreen()
        {
            IsTransitionScreen = true;
            switch (_loadScreen)
            {
                case ScreenTransition.Battle:
                    foreach (SpriteRenderer sprite in _listSprite)
                    {
                        sprite.color = new Color(0, 0, 0, 1);
                    }
                    break;
                case ScreenTransition.Ocarina:
                    foreach (SpriteRenderer sprite in _listSprite)
                    {
                        sprite.color = new Color(0, 0, 1, 1);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
