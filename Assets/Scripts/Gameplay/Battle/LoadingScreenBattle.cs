using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ScreenColorTransitionSO _screenColor;
        [SerializeField] private List<SpriteRenderer> _spriteList;
        [SerializeField] private string _parameterLoadingScreen;

        public void LoadBattleScreen()
        {
            foreach (SpriteRenderer sprite in _spriteList)
            {
                sprite.color = _screenColor.ColorLoadingScreen;
            }
            _animator.ResetTrigger(_parameterLoadingScreen);
            _animator.SetTrigger(_parameterLoadingScreen);
        }
    }
}
