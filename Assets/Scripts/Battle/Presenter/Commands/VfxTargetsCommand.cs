using System.Collections;
using System.Linq;
using CryptoQuest.Battle.VFX;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Battle.Presenter.Commands
{
    internal class VfxTargetsCommand : IPresentCommand
    {
        private AbilitySystemBehaviour[] _targets;
        private readonly int _vfxId;
        private readonly VFXPresenter _presenter;

        public VfxTargetsCommand(int vfxId, VFXPresenter presenter,
            params AbilitySystemBehaviour[] targets)
        {
            _vfxId = vfxId;
            _presenter = presenter;
            _targets = targets;
        }

        public IEnumerator Present()
        {
            if (_vfxId < 0 || _targets.Length <= 0) yield break;

            var transformPosition = Vector3.zero;
            var prefab = _presenter.GetVfxPrefab(_vfxId);
            
            var positionCalculator = prefab.GetComponent<IPositionCalculator>();
            if (positionCalculator != null)
            {
                transformPosition = positionCalculator.CalculatePosition(_targets.Select(t => t.transform).ToList());
            }

            yield return _presenter.PresentVfx(_vfxId, transformPosition);
        }
    }
}