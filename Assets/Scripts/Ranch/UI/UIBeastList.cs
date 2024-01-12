using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastList : UIBoxList<UIBeastItem>
    {
        public List<UIBeastItem> SelectedItems
        {
            get
            {
                var beasts = GetComponentsInChildren<UIBeastItem>();
                var beastsToTransfer = new List<UIBeastItem>();
                foreach (var uiBeast in beasts)
                {
                    if (!uiBeast.MarkedForTransfer) continue;
                    beastsToTransfer.Add(uiBeast);
                }

                return beastsToTransfer;
            }
        }

        public void Initialize(BeastResponse[] beastResponses)
        {
            Clear();
            foreach (var beast in beastResponses)
            {
                if (beast.id == -1) continue;
                var uiBeast = GetItem();
                uiBeast.Initialize(beast);
            }
        }

        public void Reset()
        {
            foreach (UIBeastItem uiBeast in GetComponentsInChildren<UIBeastItem>())
            {
                uiBeast.MarkedForTransfer = false;
            }
        }

        protected override void OnRelease(UIBeastItem item)
        {
            base.OnRelease(item);
            item.MarkedForTransfer = false;
        }

        protected override void OnGet(UIBeastItem uiItem)
        {
            base.OnGet(uiItem);
            uiItem.MarkedForTransfer = false;
        }
    }
}