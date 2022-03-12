using System;
using System.Linq;

namespace Commands
{
    public class EndTurnCommand : Command
    {
        public static event Action<EndTurnCommand> Callbacks; 
        public override bool Validate()
        {
            return true;
        }

        public override void Execute()
        {
            GameStatus.CurrentSide = GameStatus.OtherSide;
            
            if (GameStatus.CurrentSide == GameStatus.WentFirst)
            {
                GameStatus.RoundCount++;
            }
            GameStatus.CurrentSideMana = GameStatus.RoundCount;
            
            GameManager.CommandQueue.Enqueue(new DrawCardCommand(GameStatus.CurrentSide));
            
            // i have no idea how this works but it does
            foreach (
                BasicCard card in from row in GameManager.CurrentSideRows from card in row.cardSlots 
                where card.cardComponent is { } select card)
            {
                GameManager.CommandQueue.Enqueue(new AttackStateCommand(card.cardComponent, 1));
            }
            foreach (
                BasicCard card in from row in GameManager.OtherSideRows from card in row.cardSlots 
                where card.cardComponent is { } select card)
            {
                GameManager.CommandQueue.Enqueue(new AttackStateCommand(card.cardComponent, 0));
            }
            
            GameManager.Instance.UpdateUI();
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
