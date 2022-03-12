using System;

namespace Commands
{
    public class AttackStateCommand : Command
    {
        private readonly Card _card;
        private readonly int _canAttack;

        public static event Action<AttackStateCommand> Callbacks; 

        private bool CanAttack => _canAttack == 1;

        public AttackStateCommand(Card card, int canAttack)
        {
            _card = card;
            _canAttack = canAttack;
        }

        public override bool Validate()
        {
            return _card.canAttack != CanAttack;
        }

        public override void Execute()
        {
            _card.canAttack = CanAttack;
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
