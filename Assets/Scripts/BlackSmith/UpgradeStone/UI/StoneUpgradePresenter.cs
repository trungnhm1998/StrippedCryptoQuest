using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class StoneUpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradableStoneList _upgradableStoneListUI;
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }
    }
}