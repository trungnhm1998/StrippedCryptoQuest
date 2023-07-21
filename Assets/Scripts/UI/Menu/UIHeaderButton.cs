using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu
{
    public class UIHeaderButton : MultiInputButton
    {
        [SerializeField] private GameObject _pointer;

        public override void OnSelect(BaseEventData eventData)
        {
            _pointer.SetActive(true);
            base.OnSelect(eventData);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            _pointer.SetActive(false);
            base.OnDeselect(eventData);
        }
    }
}