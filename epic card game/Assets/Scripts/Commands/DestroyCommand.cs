using System;
using Object = UnityEngine.Object;

namespace Commands
{

    public class DestroyCommand : Command
    {
        public static event Action<DestroyCommand> Callbacks; 
        private readonly BasicCard _whatToDestroy;
        /// <summary>
        /// <param name="whatToDestroy">What to destroy</param>
        /// </summary>
        public DestroyCommand(BasicCard whatToDestroy)
        {
            _whatToDestroy = whatToDestroy;
        }

        public override bool Validate()
        {
            return true;
        }

        public override void Execute()
        {
            if (_whatToDestroy == GameManager.Instance.enemyPlayer || _whatToDestroy == GameManager.Instance.friendlyPlayer)
            {
                GameManager.CommandQueue.Enqueue(new PlayerOutCommand(_whatToDestroy.cardComponent.side));
            }
            
            CardRow parent = _whatToDestroy.parent;
            parent.RemoveCard(_whatToDestroy);
            Object.Destroy(_whatToDestroy.gameObject);
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
