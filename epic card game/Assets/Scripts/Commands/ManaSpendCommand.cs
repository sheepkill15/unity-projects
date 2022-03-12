using System;

namespace Commands
{
    public class ManaSpendCommand : Command
    {
        public static event Action<ManaSpendCommand> Callbacks; 
        private readonly Card _card;

        public ManaSpendCommand(Card card)
        {
            _card = card;
        }

        public override bool Validate()
        {
            return (_card.side == GameStatus.Side.Friendly
                ? GameStatus.FriendlyMana
                : GameStatus.EnemyMana) >= _card.stats.cost;
        }

        public override void Execute()
        {
            if (_card.side == GameStatus.Side.Friendly)
            {
                GameStatus.FriendlyMana -= _card.stats.cost;
            }
            else
            {
                GameStatus.EnemyMana -= _card.stats.cost;
            }
            GameManager.Instance.UpdateUI();
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
