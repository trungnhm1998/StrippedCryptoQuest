using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Equipment
{
    public class StoneBase : ActionBase
    {
        public int EquipmentID { get; set; }
        public List<int> StoneIDs { get; set; }
    }

    public class AttachStones : StoneBase { }
    public class AttachSucceeded : StoneBase { }
    public class DetachStones : StoneBase { }
    public class DetachSucceeded : StoneBase { }

    public class EquipmentUpdated : ActionBase
    {
        public IEquipment Equipment { get; private set; }

        public EquipmentUpdated(IEquipment equipment)
        {
            Equipment = equipment;
        }
    }
}