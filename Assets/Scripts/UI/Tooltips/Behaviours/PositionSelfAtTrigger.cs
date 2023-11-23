using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class PositionSelfAtTrigger : TooltipBehaviourBase
    {
        private void OnEnable()
            => SetPositionAtSelectedGameObject();

        private void SetPositionAtSelectedGameObject()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var config = selectedGameObject.GetComponent<TooltipConfig>();
            if (config == null) return;
            RectTransform.position = config.Default.Position;
        }

        private void Update() => SetPositionAtSelectedGameObject();
    }
}