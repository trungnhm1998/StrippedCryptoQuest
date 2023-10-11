namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public interface IGameplayEffectPolicy
    {
        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec);
    }
}