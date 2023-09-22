using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class RealEquipmentModel : MonoBehaviour, IGameEquipmentModel
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