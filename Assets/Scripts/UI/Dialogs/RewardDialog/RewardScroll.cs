using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.System.Dialogue.Builder;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class RewardScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _arrowUpHint;
        [SerializeField] private GameObject _arrowDownHint;
        private float _number;

        private void OnEnable()
        {
            CheckHintArrow();
        }

        public void HandleNavigate(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.y > 0)
            {
                ScrollUp();
            }
            if (direction.y < 0)
            {
                ScrollDown();
            }
            CheckHintArrow();
        }

        public void UpdateStep()
        {
            _number = 1f / (_scrollRect.content.childCount);
        }

        private void CheckHintArrow()
        {
            _arrowDownHint.SetActive(_scrollRect.verticalNormalizedPosition >= 0);
            _arrowUpHint.SetActive(_scrollRect.verticalNormalizedPosition <= 1f && _scrollRect.verticalNormalizedPosition > 0);
        }

        private void ScrollUp()
        {
            _scrollRect.verticalNormalizedPosition += _number;
        }

        private void ScrollDown()
        {
            _scrollRect.verticalNormalizedPosition -= _number;
        }
    }
}
