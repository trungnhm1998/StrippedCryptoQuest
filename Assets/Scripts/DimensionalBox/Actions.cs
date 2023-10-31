using CryptoQuest.Core;

namespace CryptoQuest.DimensionalBox
{
    public class GetNftEquipments : ActionBase
    {
        public bool ForceRefresh { get; set; } = false;
    }

    public class GetNftEquipmentsSucceed : ActionBase { }

    public class GetNftEquipmentsFailed : ActionBase { }
}