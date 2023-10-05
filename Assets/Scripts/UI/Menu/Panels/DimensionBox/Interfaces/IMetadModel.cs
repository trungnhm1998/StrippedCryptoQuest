
using System.Collections;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IMetadModel
    {
        public event UnityAction<float, float> CurrencyUpdated;
        public event UnityAction OnSendSuccess;
        public event UnityAction OnSendFailed;
        public float GetIngameMetad();
        public float GetWebMetad();
        IEnumerator CoLoadData();
    }
}