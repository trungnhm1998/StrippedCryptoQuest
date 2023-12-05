using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class EvolveEquipmentAction : ActionBase
    {
        public string EquipmentId;
        public string MaterialId;
    }

    public class EvolveEquipmentFailedAction : ActionBase { }

    public class EvolveEquipmentSuccessAction : ActionBase { }
}
