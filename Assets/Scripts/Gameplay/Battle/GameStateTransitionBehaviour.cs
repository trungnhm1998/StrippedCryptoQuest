using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class GameStateTransitionBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ScreenColorTransitionSO _screenColor;
        [SerializeField] private List<SpriteRenderer> _spriteList;
        private static readonly int AnimIsLoadingScreen = Animator.StringToHash("IsLoadingScreen");

        public void LoadBattleScreen()
        {
            foreach (SpriteRenderer sprite in _spriteList)
            {
                sprite.color = _screenColor.ColorLoadingScreen;
            }
            _animator.ResetTrigger(AnimIsLoadingScreen);
            _animator.SetTrigger(AnimIsLoadingScreen);
        }
    }
}
