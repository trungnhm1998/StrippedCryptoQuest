using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
{
    public class CurrencyAmountChangedAction : ActionBase
    {
        public int Amount { get; }

        public CurrencyAmountChangedAction(int amount)
        {
            Amount = amount;
        }
    }

    public class AddGoldAction : CurrencyAmountChangedAction
    {
        public AddGoldAction(int amount) : base(amount) { }
    }

    public class RemoveGoldAction : CurrencyAmountChangedAction
    {
        public RemoveGoldAction(int amount) : base(amount) { }
    }

    public class AddDiamonds : CurrencyAmountChangedAction
    {
        public AddDiamonds(int amount) : base(amount) { }
    }

    public class RemoveDiamonds : CurrencyAmountChangedAction
    {
        public RemoveDiamonds(int amount) : base(amount) { }
    }

    public class AddSouls : CurrencyAmountChangedAction
    {
        public AddSouls(int amount) : base(amount) { }
    }

    public class RemoveSouls : CurrencyAmountChangedAction
    {
        public RemoveSouls(int amount) : base(amount) { }
    }

    public class SetGoldAction : CurrencyAmountChangedAction
    {
        public SetGoldAction(int amount) : base(amount) { }
    }


    public class SetDiamondAction : ActionBase
    {
        public float Amount { get; }

        public SetDiamondAction(float amount)
        {
            Amount = amount;
        }
    }

    public class SetSoulAction : CurrencyAmountChangedAction
    {
        public SetSoulAction(int amount) : base(amount) { }
    }
}