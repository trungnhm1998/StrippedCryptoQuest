using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces
{
    public interface IEquipmentModel
    {
        public List<IData> Data { get; }
        public IEnumerator CoGetData();
    }
}