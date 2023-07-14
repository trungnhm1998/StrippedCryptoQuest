using UnityEngine;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
namespace CryptoQuest.UI.Battle
{
    public class CharacterList : MonoBehaviour
    {
        [SerializeField] private CharacterInfoBase[] _characterInfos;

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
    }
}