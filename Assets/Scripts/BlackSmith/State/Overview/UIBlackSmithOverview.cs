using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmithOverview : MonoBehaviour
    {
        public event Action OnUpgradeButtonPressed;
        public event Action OnEvolveButtonPressed;

        [SerializeField] private Button _defaultSelection;
        [SerializeField] private GameObject _content;

        public void Show()
        {
            StartCoroutine(CoSelectDefault());
            SetActiveOverviewUI(true);
        }

        public void Hide()
        {
            SetActiveOverviewUI(false);
            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void SetActiveOverviewUI(bool value)
        {
            _content.SetActive(value);
        }

        private IEnumerator CoSelectDefault()
        {
            yield return null;
            _defaultSelection.Select();
        }

        public void PressUpgradeButton()
        {
            OnUpgradeButtonPressed?.Invoke();
        }

        public void PressEvolveButton()
        {
            OnEvolveButtonPressed?.Invoke();
        }
    }
}