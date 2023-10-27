using CryptoQuest.Tavern.UI;
using UnityEngine;

namespace CryptoQuest.Tavern
{
    public class TavernOverviewPresenter : MonoBehaviour
    {
        [SerializeField] private UIOverview _tavernUiOverview;
        public UIOverview TavernUiOverview => _tavernUiOverview;
    }
}