
using TinyMessenger;

namespace CryptoQuest.Battle.Events
{
    public class BattleEvent : ITinyMessage
    {
        public object Sender { get; } = null;
    }

    public class RoundEndedEvent : BattleEvent { }

    public class IndexEvent : BattleEvent 
    {
        public int Index { get; set; }
    }
    
    public class SelectedDetailButtonEvent : IndexEvent { } 
    
    public class DeSelectedDetailButtonEvent : IndexEvent { } 
}