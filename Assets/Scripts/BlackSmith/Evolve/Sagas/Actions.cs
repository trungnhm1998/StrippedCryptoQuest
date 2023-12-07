using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class RequestEvolveEquipment : ActionBase
    {
        public IEquipment Equipment;
        public IEquipment Material;
    }

    public class EvolveResponsed : ActionBase
    {
        public EvolveResponse Response { get; private set; }
        public RequestEvolveEquipment RequestContext { get; private set; }

        public EvolveResponsed(EvolveResponse response, RequestEvolveEquipment requestCtx)
        {
            Response = response;
            RequestContext = requestCtx;
        }
    }

    public class RemoveEquipments : ActionBase
    {
        public List<IEquipment> Equipments { get; private set; }
        public RemoveEquipments(List<IEquipment> equipments)
        {
            Equipments = equipments;
        }
    }

    public class AddEquipment : ActionBase
    {
        public EquipmentResponse EquipmentData { get; private set; }
        public AddEquipment(EquipmentResponse equipmentData)
        {
            EquipmentData = equipmentData;
        }
    }

    public class EvolveRequestFailed : ActionBase { }

    public class EvolveEquipmentFailedAction : ActionBase { }

    public class EvolveEquipmentSuccessAction : ActionBase { }
}
