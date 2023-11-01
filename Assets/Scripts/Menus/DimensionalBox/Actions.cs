using CryptoQuest.Core;
using CryptoQuest.Menus.DimensionalBox.Objects;

namespace CryptoQuest.Menus.DimensionalBox
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