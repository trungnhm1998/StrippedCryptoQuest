using CryptoQuest.Merchant;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class PartyOrganizePanel : MonoBehaviour
    {
        [SerializeField] private MerchantInput _input;
        [SerializeField] private GameObject _content;
        [SerializeField] private UICharactersParty _uiCharactersParty;
        [SerializeField] private UICharacterInventoryList _uiCharacterInventoryList;

        private void OnEnable()
        {
            _content.SetActive(true);
            _input.NavigateEvent += SwitchFocusPanel;
        }

        private void OnDisable()
        {
            _input.NavigateEvent -= SwitchFocusPanel;
            _content.SetActive(false);
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
    }
}