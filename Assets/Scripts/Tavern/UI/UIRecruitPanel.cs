using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UIRecruitPanel : MonoBehaviour
    {
        [SerializeField] private UICharactersParty _uiCharactersParty;
        [SerializeField] private UICharacterInventoryList _uiCharacterInventoryList;

        private void OnEnable()
        {
            _uiCharactersParty.Closed += OnClose;
        }

        private void OnDisable()
        {
            _uiCharactersParty.Closed -= OnClose;
        }

        private void SwitchFocusPanel(Vector2 axis)
        {
            return;
            GameObject next = axis.x switch
            {
                > 0 => _uiCharacterInventoryList.GetComponentInChildren<Selectable>().gameObject,
                < 0 => _uiCharactersParty.GetComponentInChildren<Selectable>().gameObject,
                _ => null
            };

            if (next == null) return;
            EventSystem.current.SetSelectedGameObject(next);
        }

        public event Action Closed;

        private void OnClose() => Closed?.Invoke();
    }
}