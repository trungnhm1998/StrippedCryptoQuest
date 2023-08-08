using System;
using UnityEngine;
using DG.Tweening;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class FlashVFXBehaviour : BattleVFXBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _flashSpeed = 0.5f;

        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        private void Start()
        {
            _spriteRenderer.color = _startColor;
            _spriteRenderer.DOColor(_endColor, _flashSpeed).OnComplete(FinishVFX);
        }
    }
}