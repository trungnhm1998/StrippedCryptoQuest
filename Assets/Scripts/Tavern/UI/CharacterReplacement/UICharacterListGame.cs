﻿using System.Collections.Generic;
using CryptoQuest.Tavern.Interfaces;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterListGame : UICharacterList
    {
        private List<IGameCharacterData> _gameCharacterList = new List<IGameCharacterData>();

        public void SetGameData(List<IGameCharacterData> data, bool isGameEquipmentListEmpty = false)
        {
            _gameCharacterList = data;
            AfterSaveData(isGameEquipmentListEmpty);
        }

        protected override void RenderData()
        {
            foreach (var itemData in _gameCharacterList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRectContent).GetComponent<UITavernItem>();
                item.SetItemInfo(itemData);
                SetParentIdentity(item);
            }
        }
    }
}