using UnityEngine;
using CryptoQuest.Gameplay.Battle;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleCharacterList : MonoBehaviour
    {
        [SerializeField] private BattleManager _battleManager;
        [SerializeField] private UICharacterInfo[] _uiCharacterInfos;

        private void Start()
        {
            InitUI();
        }

        private void InitUI()
        {
            int index = 0;
            int memberCount = _battleManager.BattleTeam1.Members.Count;
            for (int i = 0; i < _uiCharacterInfos.Length; i++)
            {
                var characterInfo = _uiCharacterInfos[i];
                var isInMemberRange = i < memberCount;
                characterInfo.gameObject.SetActive(isInMemberRange);
                if (!isInMemberRange) return;
                characterInfo.SetOwnerSystem(_battleManager.BattleTeam1.Members[i]);
            }
        }
    }
}