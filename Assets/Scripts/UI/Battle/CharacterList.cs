using UnityEngine;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CharacterInfo;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class CharacterList : MonoBehaviour
    {
        [SerializeField] private CharacterInfoBase[] _characterInfos;

        private void OnValidate()
        {
            _characterInfos = gameObject.GetComponentsInChildren<CharacterInfoBase>(true);
        }

        public virtual void InitUI(List<IBattleUnit> units)
        {
            int memberCount = units.Count;
            for (int i = 0; i < _characterInfos.Length; i++)
            {
                var characterInfo = _characterInfos[i];
                var isInMemberRange = i < memberCount;
                characterInfo.gameObject.SetActive(isInMemberRange);
                if (!isInMemberRange) continue;
                characterInfo.SetData(units[i].UnitData);
            }
        }

        public virtual void SetSelectedData(string name)
        {
            foreach (var characterInfo in _characterInfos)
            {
                characterInfo.ShowSelected(name);
            }
        }

        public void SelectFirstHero()
        {
            _characterInfos[0].GetComponent<Button>().Select();
        }
    }
}