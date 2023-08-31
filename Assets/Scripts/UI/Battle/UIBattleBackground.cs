using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleBackground : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private Image _backgroundImage;
        private Sprite _backgroundSprite;

        private void Awake()
        {
            _backgroundImage.sprite = _battleBus.CurrentBattleInfo.BattleBackground;
        }
    }
}