using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmithCurrency : MonoBehaviour
    {
        [SerializeField] private Text _currentGold;
        [SerializeField] private Text _currentDiamond;

        public void UpdateCurrency(float gold, float diamond)
        {
            _currentGold.text = gold.ToString();
            _currentDiamond.text = diamond.ToString();
        }
    }
}