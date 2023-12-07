using System.Collections.Generic;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using MagicStone = CryptoQuest.Sagas.Objects.MagicStone;

namespace CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer
{
    public class UIMagicStoneList : UIDimensionalBoxList<UIMagicStone>
    {
        public List<UIMagicStone> SelectedItems
        {
            get
            {
                var stones = GetComponentsInChildren<UIMagicStone>();
                var stonesToTransfer = new List<UIMagicStone>();
                foreach (var uiMagicStone in stones)
                {
                    if (!uiMagicStone.MarkedForTransfer) continue;
                    stonesToTransfer.Add(uiMagicStone);
                }

                return stonesToTransfer;
            }
        }

        public void Initialize(MagicStone[] magicStones)
        {
            Clear();
            foreach (var magicStone in magicStones)
            {
                if (magicStone.id == -1) continue;
                var uiMagicStone = GetItem();
                uiMagicStone.Initialize(magicStone);
            }
        }

        public void Reset()
        {
            foreach (UIMagicStone uiMagicStone in GetComponentsInChildren<UIMagicStone>())
            {
                uiMagicStone.MarkedForTransfer = false;
            }
        }

        protected override void OnRelease(UIMagicStone item)
        {
            base.OnRelease(item);
            item.MarkedForTransfer = false;
        }

        protected override void OnGet(UIMagicStone uiItem)
        {
            base.OnGet(uiItem);
            uiItem.MarkedForTransfer = false;
        }
    }
}