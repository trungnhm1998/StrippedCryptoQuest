using System.Collections.Generic;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Equipment
{
    public class AttachStones : ActionBase
    {
        public int EquipmentID { get; set; }
        public List<int> StoneIDs { get; set; }
    }
    public class AttachSucceeded : ActionBase { }

    public class DetachStones : ActionBase
    {
        public int EquipmentID { get; set; }
        public List<int> StoneIDs { get; set; }
    }
    public class DetachSucceeded : ActionBase { }
}