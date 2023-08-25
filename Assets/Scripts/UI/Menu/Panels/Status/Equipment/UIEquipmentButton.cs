using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentButton : MultiInputButton, ICell
    {
        public static event UnityAction<Button> InspectingRow;

        [SerializeField] private GameObject _selectEffect;

        public void OnPressed()
        {
            Debug.Log($"Inventory item pressed");
        }

        private bool _selecting = false;

        public override void OnSelect(BaseEventData eventData)
        {
            _selecting = true;
            base.OnSelect(eventData);
            _selectEffect.SetActive(true);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            _selecting = false;
            base.OnDeselect(eventData);
            _selectEffect.SetActive(false);
        }

        private const float TIME_TIL_INSPECT = 0.5f; // TODO: move this into a config singleton
        private float _timer = TIME_TIL_INSPECT;

        private void Update()
        {
            if (!_selecting) return;
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = TIME_TIL_INSPECT;
                InspectingRow?.Invoke(this);
            }
        }
    }
}