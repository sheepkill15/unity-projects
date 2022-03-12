using System;

namespace Commands
{

    public class AddCommand : Command
    {
        public static event Action<AddCommand> Callbacks; 
        private readonly BasicCard _whatToAdd;
        private readonly CardRow _whereToAdd;
        private readonly int _oldIndex;
        
        /// <summary>
        /// <param name="whatToAdd">What to add</param>
        /// <param name="whereToAdd">Where to add</param>
        /// <param name="oldIndex">Index</param>
        /// </summary>
        public AddCommand(BasicCard whatToAdd, CardRow whereToAdd, int oldIndex)
        {
            _whatToAdd = whatToAdd;
            _whereToAdd = whereToAdd;
            _oldIndex = oldIndex;
        }

        public override bool Validate()
        {
            return true;
        }

        public override void Execute()
        {
            _whatToAdd.parent.RemoveCard(_whatToAdd);
            _whereToAdd.AddCard(_whatToAdd, _oldIndex);
        }

        public override void Invoke()
        {
            Callbacks?.Invoke(this);
        }
    }
}
