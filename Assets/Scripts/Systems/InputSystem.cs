using JetBrains.Annotations;

namespace Systems
{
    public class InputSystem
    {
        [CanBeNull] private InputSystemActions _inputSystemActions { get; }

        public InputSystemActions.PlayerActions Player => _inputSystemActions?.Player ?? new InputSystemActions.PlayerActions();

        public InputSystem()
        {
            _inputSystemActions = new InputSystemActions();
        }
    }
}
