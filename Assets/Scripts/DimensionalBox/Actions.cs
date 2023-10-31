using CryptoQuest.Core;
using CryptoQuest.DimensionalBox.Objects;

namespace CryptoQuest.DimensionalBox
{
    public class GetNftEquipments : ActionBase
    {
        public EDimensionalBoxStatus Status { get; set; } = EDimensionalBoxStatus.All;
        public bool ForceRefresh { get; set; } = false;
    }

    public class GetNftEquipmentsSucceed : ActionBase { }

    public class GetNftEquipmentsFailed : ActionBase { }

    public class TransferSucceed : ActionBase { }
}