using System;

namespace Commands
{
    public class BuffCommand : Command
    {
        public static event Action<BuffCommand> Callbacks; 
        private readonly Card _card;
        private readonly int _amount;

        public BuffCommand(Card card, int amount)
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
            _card.stats.damage += _amount;
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}