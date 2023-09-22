using CryptoQuest.Menu;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIGameEquipmentList : UIEquipmentList
    {
        protected override void SetDefaultSelection()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }
    }
}
