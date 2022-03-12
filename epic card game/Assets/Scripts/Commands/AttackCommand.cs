using System;

namespace Commands
{
    public class AttackCommand : Command
    {
        public static event Action<AttackCommand> Callbacks; 
        private readonly Card _which;
        private readonly Card _what;

        public AttackCommand(Card which, Card what)
        {
            _which = which;
            _what = what;
        }

        public override bool Validate()
        {
            return _which.canAttack;
        }

        public override void Execute()
        {
            GameManager.CommandQueue.Enqueue(new HealCommand(_what, -_which.stats.damage));
            GameManager.CommandQueue.Enqueue(new HealCommand(_which, -_what.stats.damage));
            GameManager.CommandQueue.Enqueue(new AttackStateCommand(_which, 0));
        }
        
        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}