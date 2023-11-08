using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIMetadSourceButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject _arrow;

        public void OnSelect(BaseEventData eventData)
        {
            _arrow.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _arrow.SetActive(false);
        }
    }
}