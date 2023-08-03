using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleBackground : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private Image _backgroundImage;
        private Sprite _backgroundSprite;

        private void Awake()
        {
            StartCoroutine(GetBackground());
        }

        public IEnumerator GetBackground()
        {
            AssetReferenceT<Sprite> backgroundSpriteAsset = _battleBus.CurrentBattleInfo.BattleBackground;
            if (backgroundSpriteAsset == null)
                yield break;
            AsyncOperationHandle<Sprite> handle = backgroundSpriteAsset.LoadAssetAsync<Sprite>();
            if (handle.IsValid() && backgroundSpriteAsset.Asset != null)
            {
                _backgroundSprite = backgroundSpriteAsset.Asset as Sprite;
            }

            yield return handle;
            if (handle.IsDone)
            {
                _backgroundSprite = handle.Result;
                _backgroundImage.sprite = _backgroundSprite != null ? _backgroundSprite : _backgroundImage.sprite;
            }
        }
    }
}