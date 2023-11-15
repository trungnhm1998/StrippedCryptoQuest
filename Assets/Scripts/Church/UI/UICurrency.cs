using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Church.UI
{
    public class UICurrency : MonoBehaviour
    {
        [SerializeField] private Text _currentGold;
        public void UpdateCurrency(float gold)
        {
            _currentGold.text = gold.ToString();
        }
    }
}
