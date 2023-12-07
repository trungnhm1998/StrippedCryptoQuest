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

        [field: SerializeField] public Button ButtonUI { get; private set; }
        [field: SerializeField] public GameObject BaseTag { get; private set; }

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;

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
            BaseTag.SetActive(false);
            ButtonUI.interactable = true;
        }

        public void SubmitEquipment()
        {
            Selected?.Invoke(this);
        }
    }
}
