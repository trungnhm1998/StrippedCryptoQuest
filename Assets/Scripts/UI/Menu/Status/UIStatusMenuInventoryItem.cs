using System;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenuInventoryItem : MonoBehaviour, ICell
    {
        [Serializable]
        public class Data
        {
            public LocalizedString Name;

            public Data Clone()
            {
                return new Data()
                {
                    Name = Name
                };
            }
        }

        [SerializeField] LocalizeStringEvent _name;
        [SerializeField] Text _itemOrder;
        [SerializeField] private GameObject _selectEffect;

        public void Select()
        {
            _selectEffect.SetActive(true);
        }

        public void Deselect()
        {
            _selectEffect.SetActive(false);
        }

        public void Init(Data data, int index)
        {
            _name.StringReference = data.Name;
            _itemOrder.text = index.ToString();
        }
    }
}
