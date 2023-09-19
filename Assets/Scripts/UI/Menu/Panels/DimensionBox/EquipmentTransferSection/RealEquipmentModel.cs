using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class RealEquipmentModel : MonoBehaviour, IEquipmentModel
    {
        public List<IData> Data => throw new NotImplementedException();

        public IEnumerator CoGetData(Action<List<IData>> dataCallback)
        {
            throw new NotImplementedException();
        }

        public IEnumerator CoGetData()
        {
            throw new NotImplementedException();
        }
    }
}