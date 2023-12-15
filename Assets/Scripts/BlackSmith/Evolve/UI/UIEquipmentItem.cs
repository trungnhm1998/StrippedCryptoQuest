using System;
using CryptoQuest.BlackSmith.Commons.UI;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public interface IEvolvableEquipmentItem
    {
        IEquipment Equipment { get; }
        ICurrencyValueEnough GoldCheck { get; }
        ICurrencyValueEnough DiamondCheck { get; }
    }

    public class EvolvableEquipmentItem : IEvolvableEquipmentItem
    {
        public IEquipment Equipment { get; set; }
        public ICurrencyValueEnough GoldCheck { get; set; }
        public ICurrencyValueEnough DiamondCheck { get; set; }
    }

    public class UIEquipmentItem : MonoBehaviour
    {
        public event UnityAction<UIEquipmentItem> Selected;
        public event Action<UIEquipmentItem> Highlighted;

        [field: SerializeField] public MultiInputButton ButtonUI { get; private set; }
        [field: SerializeField] public GameObject BaseTag { get; private set; }
        [field: SerializeField] public GameObject MaterialTag { get; private set; }
        [field: SerializeField] public Image SelectedImage { get; private set; }
        [field: SerializeField] public Color SelectedColor { get; private set; }
        private Color _originalColor;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private UIStars _stars;
        [SerializeField] private UICurrencyValueEnoughText _goldText;
        [SerializeField] private UICurrencyValueEnoughText _diamondText;

        public IEquipment Equipment { get; private set; }
        public ICurrencyValueEnough GoldCheck { get; private set; }
        public ICurrencyValueEnough DiamondCheck { get; private set; }

        private void Awake()
        {
            _originalColor = SelectedImage.color;
        }

        private void OnEnable()
        {
            ButtonUI.Selected += HandleButtonHighlight;
        }

        private void OnDisable()
        {
            ButtonUI.Selected -= HandleButtonHighlight;
        }

        public void Init(IEvolvableEquipmentItem item)
        {
            ResetItemStates();

            Equipment = item.Equipment;
            GoldCheck = item.GoldCheck;
            DiamondCheck = item.DiamondCheck;
            _icon.sprite = item.Equipment.Type.Icon;
            _nameLocalize.StringReference = item.Equipment.DisplayName;
            _stars.SetStars(item.Equipment.Data.Stars);
            _goldText.SetValueAndCheckIsEnough(item.GoldCheck);
            _diamondText.SetValueAndCheckIsEnough(item.DiamondCheck);
        }

        public void ResetItemStates()
        {
            BaseTag.SetActive(false);
            MaterialTag.SetActive(false);
            SetSelected(false);
            ButtonUI.interactable = true;
        }

        public void SubmitEquipment()
        {
            if (GoldCheck.IsEnough && DiamondCheck.IsEnough)
                Selected?.Invoke(this);
        }

        public void SetSelected(bool selected)
        {
            SelectedImage.gameObject.SetActive(selected);
            SelectedImage.color = selected ? SelectedColor : _originalColor;
        }

        private void HandleButtonHighlight()
        {
            Highlighted?.Invoke(this);
        }
    }
}
