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
            var battlefield = _bus.CurrentBattlefield;
            var encounter = _bus.CurrentEncounter;

            AssetReferenceT<Sprite> defaultBackground =
                (encounter != null && !string.IsNullOrEmpty(encounter.Background?.AssetGUID))
                    ? encounter.Background
                    : null;

            AssetReferenceT<Sprite> background =
                (battlefield != null && !string.IsNullOrEmpty(battlefield.Background?.AssetGUID))
                    ? battlefield.Background
                    : defaultBackground;


            if (background == null || string.IsNullOrEmpty(background.AssetGUID))
                yield break;

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