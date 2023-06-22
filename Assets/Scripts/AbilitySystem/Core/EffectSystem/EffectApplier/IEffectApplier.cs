namespace Indigames.AbilitySystem
{
    public interface IEffectApplier
    {
        public void ApplyInstantEffect(AbstractEffect abstractEffect);
        public void ApplyDurationalEffect(AbstractEffect durationalAbstractEffect);
    }
}