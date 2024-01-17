using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
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

    public class StonePassiveRequestBase : StoneBase
    {
        public int CharacterID { get; set; }
        public MagicStonePassiveController PassiveController { get; set; }
    }

    public class ApplyStonePassiveRequest : StonePassiveRequestBase { }


    public class RemoveStonePassiveRequest : StonePassiveRequestBase { }

    public class EquipmentUpdated : ActionBase
    {
        public IEquipment Equipment { get; private set; }

        public EquipmentUpdated(IEquipment equipment)
        {
            Equipment = equipment;
        }
    }
}