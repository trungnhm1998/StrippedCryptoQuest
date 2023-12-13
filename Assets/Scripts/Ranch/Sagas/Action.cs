using CryptoQuest.Beast;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch.Sagas
{
    public class RequestEvolveBeast : ActionBase
    {
        public IBeast Base;
        public IBeast Material;
    }
}