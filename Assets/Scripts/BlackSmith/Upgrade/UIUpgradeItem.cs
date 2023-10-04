using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeItem : MonoBehaviour
    {
        public static event UnityAction<UIUpgradeItem> SelectItemEvent;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private MultiInputButton _button;


        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> equipmentTypeIcon)
        {
            if (equipmentTypeIcon.RuntimeKeyIsValid() == false) yield break;
            var handle = equipmentTypeIcon.LoadAssetAsync<Sprite>();
            yield return handle;
            _icon.sprite = handle.Result;
        }
        
        public void ConfigureCell(IUpgradeEquipment equipment)
        {
            _displayName.StringReference = equipment.DisplayName;
            _cost.text = equipment.Cost.ToString();
            StartCoroutine(LoadSpriteAndSet(equipment.Icon));
        }

        private void OnEnable()
        {
            _button.Selected += OnSeleceted;
            _button.DeSelected += OnDeselected;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSeleceted;
            _button.DeSelected -= OnDeselected;
        }

        private void OnSeleceted()
        {
            _selectedBackground.SetActive(true);
            SelectItemEvent?.Invoke(this);
        }

        private void OnDeselected()
        {
            _selectedBackground.SetActive(false);
        }
    }
}