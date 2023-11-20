using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class ConfigTooltipPivot : TooltipBehaviourBase
    {
        public override void Setup()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var config = selectedGameObject.GetComponent<TooltipConfig>();
            if (config == null) return;
            RectTransform.pivot = config.Default.Pivot;
        }
    }
}