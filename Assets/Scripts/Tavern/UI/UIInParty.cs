using System.Collections;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UIInParty : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private PartySO _party;
        [SerializeField] private UICharacterListItem _uiCharacterListItem;
        [SerializeField] private GameObject _partyIndicator;

        public bool IsInParty
        {
            get => _partyIndicator.activeSelf;
            set => _partyIndicator.SetActive(value);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _uiCharacterListItem.Spec.IsValid());
            IsInParty = _party.Exists(_uiCharacterListItem.Spec);
            if (IsInParty) _button.onClick.RemoveAllListeners();
        }
    }
}