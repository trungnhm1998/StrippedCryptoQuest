using System.Collections.Generic;
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
}