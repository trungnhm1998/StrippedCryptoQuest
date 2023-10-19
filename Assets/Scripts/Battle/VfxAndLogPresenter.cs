using CryptoQuest.Battle.UI.Logs;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IPresentCommand { }

    public class VfxAndLogPresenter : MonoBehaviour
    {
        [SerializeField] private LogPresenter _logPresenter;
        private void Awake() { }

        private void OnDestroy() { }
    }
}