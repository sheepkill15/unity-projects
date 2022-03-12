using System;

namespace Commands
{
    public class HealCommand : Command
    {
        public static event Action<HealCommand> Callbacks; 
        private readonly Card _card;
        private readonly int _amount;

        public HealCommand(Card card, int amount)
        {
            _card = card;
            _amount = amount;
        }

        public override bool Validate()
        {
            return true;
        }

        public override void Execute()
        {
            _card.stats.health += _amount;
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
