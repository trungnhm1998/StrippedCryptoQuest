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
            foreach (var member in _battleManager.BattleTeam1.Members)
            {
                var characterInfo = _uiCharacterInfos[index];
                characterInfo.gameObject.SetActive(true);
                characterInfo.SetOwnerSystem(member);
                index++;
            }

            for (int i = index; i < _uiCharacterInfos.Length; i++)
            {
                _uiCharacterInfos[i].gameObject.SetActive(false);
            }
        }
    }
}