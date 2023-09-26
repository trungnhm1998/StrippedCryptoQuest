
using System.Collections;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IMetadModel
    {
        public event UnityAction<float, float> CurrencyUpdated;
        public float GetIngameMetad();
        public float GetWebMetad();
        public void UpdateCurrency(float inputValue, bool ingameWallet);
        IEnumerator CoLoadData();
    }
}