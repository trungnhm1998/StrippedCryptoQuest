using System.Collections;
using CryptoQuest.Battle.VFX;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Battle.Presenter.Commands
{
    internal class VfxCommand : IPresentCommand
    {
        private AssetReference _visualEffect;
        private readonly Vector3 _transformPosition;
        private readonly int _vfxId;
        private readonly VFXPresenter _presenter;

        public VfxCommand(int vfxId, Vector3 transformPosition, VFXPresenter presenter)
        {
            _vfxId = vfxId;
            _presenter = presenter;
            _transformPosition = transformPosition;
        }

        public IEnumerator Present()
        {
            yield return _presenter.PresentVfx(_vfxId, _transformPosition);
        }
    }
}