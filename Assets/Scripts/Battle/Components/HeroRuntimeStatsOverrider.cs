using IndiGames.GameplayAbilitySystem.AttributeSystem;

namespace CryptoQuest.Battle.Components
{
    public class HeroRuntimeStatsOverrider : CharacterComponentBase
    {
        private IHeroSpecProvider _heroSpecProvider;

        protected override void OnInit()
        {
            base.OnInit();

            TryGetComponent(out _heroSpecProvider);

            foreach (var runtimeAttribute in _heroSpecProvider.Spec.RuntimeStats)
            {
                Character.AttributeSystem.SetAttributeValue(runtimeAttribute.Attribute, new AttributeValue()
                {
                    Attribute = runtimeAttribute.Attribute,
                    BaseValue = runtimeAttribute.Value,
                    CurrentValue = runtimeAttribute.Value,
                });
            }
        }
    }
}