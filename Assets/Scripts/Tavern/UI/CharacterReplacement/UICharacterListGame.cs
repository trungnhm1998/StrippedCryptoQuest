using System.Collections.Generic;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterListGame : UICharacterList
    {
        private List<IGameCharacterData> _gameEquipmentList = new List<IGameCharacterData>();

        public void SetGameData(List<IGameCharacterData> data, bool isGameEquipmentListEmpty = false)
        {
            _gameEquipmentList = data;
            AfterSaveData(isGameEquipmentListEmpty);
        }

        protected override void RenderData()
        {
            foreach (var itemData in _gameEquipmentList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRectContent).GetComponent<UITavernItem>();
                item.SetItemInfo(itemData);
                SetParentIdentity(item);
            }
        }
    }
}