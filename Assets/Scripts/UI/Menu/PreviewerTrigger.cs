using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu
{
    public class PreviewerTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public void OnSelect(BaseEventData eventData) { }

        public void OnDeselect(BaseEventData eventData) { }
    }
}