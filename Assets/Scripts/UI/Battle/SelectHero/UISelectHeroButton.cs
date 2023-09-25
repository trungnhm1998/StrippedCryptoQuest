using System;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Battle.SelectHero
{
    [RequireComponent(typeof(MultiInputButton))]
    public class UISelectHeroButton : MonoBehaviour
    {
        public event Action ConfirmPressed;

        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _content;
        [SerializeField] private LocalizeStringEvent _label;

        private void OnValidate()
        {
            _button = GetComponent<MultiInputButton>();
        }

        private void Awake()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(ConfirmCharacter);
        }

        public void SetUIActive(bool value)
        {
            _button.interactable = value;
            _content.SetActive(value);
        }

        public void SelectButton()
        {
            _button.Select();
        }

        public void SetLabel(LocalizedString label)
        {
            if (label == null) return;
            _label.StringReference = label;
        }

        public void SetUIPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void ConfirmCharacter()
        {
            ConfirmPressed?.Invoke();
        }
    }
}