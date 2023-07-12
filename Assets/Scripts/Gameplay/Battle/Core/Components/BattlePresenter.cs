using UnityEngine;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattlePresenter : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private CharacterList _heroesUI;
        [SerializeField] private CharacterList _monstersUI;

        private void Start()
        {
            _heroesUI.InitUI(_battleBus.BattleManager.BattleTeam1.BattleUnits);
            _monstersUI.InitUI(_battleBus.BattleManager.BattleTeam2.BattleUnits);
        }
    }   
}