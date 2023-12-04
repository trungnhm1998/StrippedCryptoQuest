using UnityEngine;
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

            var handle = spriteAsset.OperationHandle.IsValid() == false
                ? spriteAsset.LoadAssetAsync()
                : spriteAsset.OperationHandle;
            handle.Completed += _ =>
            {
                image.sprite = (Sprite)spriteAsset.OperationHandle.Result;
                image.enabled = true;
            };
            return handle;
        }
    }
}