using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IWalletEquipmentModel
    {
        public bool IsLoaded { get; }
        public List<INFT> Data { get; }
        public IEnumerator CoGetData();
        public void Transfer();
    }
}