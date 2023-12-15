using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class StoneUpgradePresenter : MonoBehaviour
    {
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }
    }
}