using CryptoQuest.AbilitySystem;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class SkillParameterBuilder
    {
        private float _basePower = 0;
        private float _powerUpperLimit = 20;
        private float _powerLowerLimit = 10;
        private float _skillPowerThreshold = 10;
        private float _powerValueAdded = 0.5f;
        private float _powerValueReduced = 0.5f;
        private int _continuesTurn = 1;

        public SkillParameters Build()
        {
            return new SkillParameters()
            {
                BasePower = _basePower,
                PowerUpperLimit = _powerUpperLimit,
                PowerLowerLimit = _powerLowerLimit,
                SkillPowerThreshold = _skillPowerThreshold,
                PowerValueAdded = _powerValueAdded,
                PowerValueReduced = _powerValueReduced,
                ContinuesTurn = _continuesTurn
            };
        }

        public SkillParameterBuilder WithBasePower(float basePower)
        {
            _basePower = basePower;
            return this;
        }

        public SkillParameterBuilder WithPowerUpperLimit(float powerUpperLimit)
        {
            _powerUpperLimit = powerUpperLimit;
            return this;
        }

        public SkillParameterBuilder WithPowerLowerLimit(float powerLowerLimit)
        {
            _powerLowerLimit = powerLowerLimit;
            return this;
        }

        public SkillParameterBuilder WithSkillPowerThreshold(float skillPowerThreshold)
        {
            _skillPowerThreshold = skillPowerThreshold;
            return this;
        }

        public SkillParameterBuilder WithPowerValueAdded(float powerValueAdded)
        {
            _powerValueAdded = powerValueAdded;
            return this;
        }

        public SkillParameterBuilder WithPowerValueReduced(float powerValueReduced)
        {
            _powerValueReduced = powerValueReduced;
            return this;
        }

        public SkillParameterBuilder WithContinuesTurn(int continuesTurn)
        {
            _continuesTurn = continuesTurn;
            return this;
        }
    }
}