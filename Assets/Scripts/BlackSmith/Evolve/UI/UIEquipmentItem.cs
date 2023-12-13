using System;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        public event UnityAction<UIEquipmentItem> Selected;
        public event Action<UIEquipmentItem> Highlighted;

        [field: SerializeField] public MultiInputButton ButtonUI { get; private set; }
        [field: SerializeField] public GameObject BaseTag { get; private set; }

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private UIStars _stars;

        public IEquipment Equipment { get; private set; }

        private void OnEnable()
        {
            ButtonUI.Selected += HandleButtonHighlight;
        }

        private void OnDisable()
        {
            ButtonUI.Selected -= HandleButtonHighlight;
        }

        public void Init(IEquipment equipment)
        {
            ResetItemStates();

            Equipment = equipment;
            _icon.sprite = equipment.Type.Icon;
            _nameLocalize.StringReference = equipment.DisplayName;
            _stars.SetStars(equipment.Data.Stars);
        }

        public void ResetItemStates()
        {
            BaseTag.SetActive(false);
            ButtonUI.interactable = true;
        }

        public void SubmitEquipment()
        {
            Selected?.Invoke(this);
        }

        private void HandleButtonHighlight()
        {
            Highlighted?.Invoke(this);
        }
    }
}
