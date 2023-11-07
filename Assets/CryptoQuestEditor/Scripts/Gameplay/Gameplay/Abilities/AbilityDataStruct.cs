public struct AbilityDataStruct
{
    public string Id;
    public string LocalizedKey;
    public string ElementId;
    public int Mp;
    public string EffectTriggerTimingId;
    public string SkillTypeId;
    public string CategoryTypeId;
    public string MainEffectTypeId;
    public string MainEffectTargetParameterId;
    public string TargetTypeId;
    public int ContinuousTurns;
    public string ValueType;
    public float BasePower;
    public float PowerUpperLimit;
    public float PowerLowerLimit;
    public float SkillPowerThreshold;
    public float PowerValueAdded;
    public float PowerValueReduced;
    public float SuccessRate;
    public string ScenarioId;
    public string VfxId;
    public SubEffectDataStruct SubEffectData;

    public bool IsSubEffectValid()
    {
        return !string.IsNullOrEmpty(this.SubEffectData.EffectTypeId);
    }
}

public struct SubEffectDataStruct
{
    public string EffectTypeId;
    public string EffectTargetParameterId;
    public string TargetTypeId;
    public int ContinuousTurns;
    public float BasePower;
}