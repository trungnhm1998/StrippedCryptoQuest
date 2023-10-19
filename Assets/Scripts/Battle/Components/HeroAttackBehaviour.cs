using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Battle.Components
{
    public class HeroAttackBehaviour : NormalAttack
    {
        [SerializeField] private AssetReference _visualEffect;

        protected override void OnPreAttack(Character target)
        {
            // var handle = _visualEffect.InstantiateAsync(target.transform);
            // yield return handle;
            // var vfx = handle.Result.GetComponent<BattleVFXBehaviour>();
            // yield return new WaitUntil(() => vfx == null);
        }
    }
}