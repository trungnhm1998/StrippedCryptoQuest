using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IGameEquipmentModel
    {
        public List<IData> Data { get; }
        public IEnumerator CoGetData();
    }
}