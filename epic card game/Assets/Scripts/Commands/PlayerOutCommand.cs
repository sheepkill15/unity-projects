using System;

namespace Commands
{
    public class PlayerOutCommand : Command
    {
        public static event Action<PlayerOutCommand> Callbacks;

        private GameStatus.Side _side;

        public PlayerOutCommand(GameStatus.Side side)
        {
            _side = side;
        }

        public override bool Validate()
        {
            // if (_side == GameManager.Side.Enemy)
            // {
            //     return GameManager.Instance.enemyPlayer.cardComponent?.stats.health == 0;
            // }
            //
            // return GameManager.Instance.friendlyPlayer.cardComponent?.stats.health == 0;
            return true;
        }

        public override void Execute()
        {
            GameManager.GameOver(_side);
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}