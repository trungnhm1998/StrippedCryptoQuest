using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest
{
    public class UIAttributePreviewableUnknown : UIAttribute
    {
        [SerializeField] private GameObject _previewIncreaseGO;

        public LocalizedString LocalizeAttributeName { get; private set; }

        public override void SetAttribute(LocalizedString attributeName, float value)
        {
            base.SetAttribute(attributeName, value);
            LocalizeAttributeName = attributeName;
        }

        public void SetPreviewUnknownValue()
        {
            _previewIncreaseGO.SetActive(true);
        }

        public void ResetPreviewUI()
        {
            _previewIncreaseGO.SetActive(false);
        }
    }
}
