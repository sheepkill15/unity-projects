using System;
using System.Linq;

namespace Commands
{
    public class DrawCardCommand : Command
    {
        public static event Action<DrawCardCommand> Callbacks; 
        private readonly GameStatus.Side _side;

        public DrawCardCommand(Card card)
        {
            _side = card.side;
        }
        
        public DrawCardCommand(GameStatus.Side side)
        {
            _side = side;
        }

        public override bool Validate()
        {
            return _side == GameStatus.Side.Friendly
                ? GameStatus.FriendlyRemainingCards.Count > 0 : GameStatus.EnemyRemainingCards.Count > 0;
        }

        public override void Execute()
        {
            CardRow targetHandRow = _side == GameStatus.Side.Friendly ? 
                GameManager.FriendlyHandRow : GameManager.EnemyHandRow;

            CardInfo newCardInfo = _side == GameStatus.Side.Friendly
                ? GameStatus.FriendlyRemainingCards.Dequeue()
                : GameStatus.EnemyRemainingCards.Dequeue();
            
            GameManager.CommandQueue.Enqueue(new AddCommand(targetHandRow.GeneratePlaceholder(newCardInfo),
                    targetHandRow, targetHandRow.numCardSlots));
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
