using System;
using UnityEngine;

namespace Commands
{
    public class PlaceCommand : Command
    {
        public static event Action<PlaceCommand> Callbacks; 
        public readonly Card WhatToPlace;
        private readonly BasicCard _whatToReplace;
        
        /// <summary>
        /// <param name="whatToPlace">What to place</param>
        /// <param name="whatToReplace">What to replace</param>
        /// </summary>
        public PlaceCommand(Card whatToPlace, BasicCard whatToReplace)
        {
            _whatToReplace = whatToReplace;
            WhatToPlace = whatToPlace;
        }

        public override bool Validate()
        {
            bool res = WhatToPlace.stats.cost <= GameStatus.CurrentSideMana &&
                       WhatToPlace.side == GameStatus.CurrentSide;
            if (!res)
            {
                WhatToPlace.state = Card.CardState.Hand;
            }

            return res;
        }

        public override void Execute()
        {
            CardRow newParent = _whatToReplace.parent;
            int newIndex = newParent.cardSlots.IndexOf(_whatToReplace);
            Vector3 targetPos = _whatToReplace.cardTransform.position;
            GameManager.CommandQueue.Enqueue(new DestroyCommand(_whatToReplace));
            GameManager.CommandQueue.Enqueue(new AddCommand(WhatToPlace.basicCardInfo, newParent, newIndex));
            GameManager.CommandQueue.Enqueue(new ManaSpendCommand(WhatToPlace));
            WhatToPlace.state = Card.CardState.Placed;
            WhatToPlace.returnPosition = targetPos;
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
