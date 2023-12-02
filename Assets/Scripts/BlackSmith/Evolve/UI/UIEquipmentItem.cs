using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        public event UnityAction<UIEquipmentItem> Selected;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private GameObject _selectedTag;

        public IEquipment Equipment { get; private set; }

        public void Init(IEquipment equipment)
        {
            ResetItemStates();

            Equipment = equipment;
            _icon.sprite = equipment.Type.Icon;
            _nameLocalize.StringReference = equipment.DisplayName;
        }

        public void ResetItemStates()
        {
            _selectedTag.SetActive(false);
        }

        public void SubmitEquipment()
        {
            _selectedTag.SetActive(true);
            Selected?.Invoke(this);
        }
    }
}