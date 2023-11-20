using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class PositionSelfAtTrigger : TooltipBehaviourBase
    {
        public override void Setup()
            => SetPositionAtSelectedGameObject();

        private void SetPositionAtSelectedGameObject()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var config = selectedGameObject.GetComponent<TooltipConfig>();
            if (config == null) return;
            RectTransform.position = config.Default.Position;
        }
    }
}