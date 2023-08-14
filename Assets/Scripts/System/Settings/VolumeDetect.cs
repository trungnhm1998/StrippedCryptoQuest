using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.System.Settings
{
    public class VolumeDetect : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Slider _volumeSlider;

        public void OnSelect(BaseEventData eventData)
        {
            _volumeSlider.handleRect.gameObject.SetActive(IsSelected(eventData));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _volumeSlider.handleRect.gameObject.SetActive(!IsSelected(eventData));
        }

        private bool IsSelected(BaseEventData eventData)
        {
            return eventData.selectedObject.gameObject == gameObject;
        }
    }
}