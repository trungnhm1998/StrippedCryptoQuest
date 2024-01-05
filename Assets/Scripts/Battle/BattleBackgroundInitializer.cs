using System.Collections;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.Battle
{
    public class BattleBackgroundInitializer : MonoBehaviour
    {
        [SerializeField] private BattleBus _bus;
        [SerializeField] private Image _backgroundImage;
        private AsyncOperationHandle _backgroundSpriteHandle;

        private IEnumerator Start()
        {
            var background = _bus.CurrentBattlefield.Background;

            if (string.IsNullOrEmpty(background.AssetGUID)) yield break;
            var handle = _backgroundImage.LoadSpriteAndSet(background);
            yield return handle;
            _backgroundSpriteHandle = handle;
        }

        private void OnDisable()
        {
            if (_backgroundSpriteHandle.IsValid())
                Addressables.Release(_backgroundSpriteHandle);
        }
    }
}