﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Extensions
{
    public static class ImageExtensions
    {
        public static AsyncOperationHandle LoadSpriteAndSet(this Image image,
            AssetReferenceT<Sprite> spriteAsset)
        {
            image.enabled = false;
            if (spriteAsset.RuntimeKeyIsValid() == false) return default;
            if (spriteAsset.OperationHandle.IsValid() && spriteAsset.OperationHandle.IsDone)
            {
                image.sprite = (Sprite)spriteAsset.OperationHandle.Result;
                image.enabled = true;
                return spriteAsset.OperationHandle;
            }
            var handle = spriteAsset.LoadAssetAsync();
            handle.Completed += handle1 =>
            {
                image.sprite = handle1.Result;
                image.enabled = true;
            };
            return handle;
        }
    }
}