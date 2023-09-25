using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.Character.Attributes.ClampEventAction
{
    public interface IClampAction
    {
        void OnClampSuccess(AttributeSystemBehaviour attributeSystem);
        void OnClampFailed(AttributeSystemBehaviour attributeSystem);
    }

    public class DoNothing : IClampAction
    {
        public void OnClampSuccess(AttributeSystemBehaviour attributeSystem) {}
        public void OnClampFailed(AttributeSystemBehaviour attributeSystem) {}
    }
}