using System;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CryptoQuest.Credits
{
    public class CreditScroller : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _timeDuration;
        [SerializeField] private UnityEvent _rollCreditEnded;
        private Sequence _sequence;

        private void OnEnable()
        {
            StartCreditRoll();
        }

        private void StartCreditRoll()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_content.transform.DOMoveY(
                0, _timeDuration).SetEase(Ease.Linear).OnComplete(_rollCreditEnded.Invoke));
        }
    }
}