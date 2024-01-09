using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.ShopSystem.PreviewCharacter
{
    public class CharactersPreviewPresenter : MonoBehaviour
    {
        [SerializeField] private List<CharacterEquipmentPreviewer> _previewers;
        [SerializeField] private ShowTooltipEvent _showEquipmentEvent;

        private void OnValidate()
        {
            _previewers =
                new(GetComponentsInChildren<CharacterEquipmentPreviewer>(true));
        }

        private void OnEnable()
        {
            _showEquipmentEvent.EventRaised += EquipmentSelected;
        }

        private void OnDisable()
        {
            _showEquipmentEvent.EventRaised -= EquipmentSelected;
        }
        
        private void EquipmentSelected(bool isShow)
        {
            if (!isShow)
            {
                ResetPreview();
                return;
            }
            
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null) return;
            var provider = selectedObject.GetComponent<ITooltipEquipmentProvider>();
            PreviewEquipment(provider.Equipment);
        }

        private void PreviewEquipment(IEquipment equipment)
        {
            foreach (var previewer in _previewers)
            {
                if (!previewer.IsValid) continue;
                previewer.PreviewEquip(equipment);
            }
        }

        private void ResetPreview()
        {
            foreach (var previewer in _previewers)
            {
                if (!previewer.IsValid) continue;
                previewer.ResetPreview();
            }
        }   
    }
}
