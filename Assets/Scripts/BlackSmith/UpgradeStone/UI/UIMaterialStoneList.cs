using System;
using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIMaterialStoneList : UIUpgradableStoneList
    {
        public event Action<UIUpgradableStone> MaterialSelected;
        public event Action ResetMaterial;

        public override void RenderStones(List<IMagicStone> items)
        {
            base.RenderStones(items);
            foreach (var cachedItem in _cachedItems)
            {
                cachedItem.MaterialTag.SetActive(true);
            }
        }

        protected override void OnSelectItem(UIUpgradableStone ui)
        {
            MaterialSelected?.Invoke(ui);
        }

        public void ResetMaterials()
        {
            ResetMaterial?.Invoke();
        }
    }
}